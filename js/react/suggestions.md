# Ревизия проекта `js/react` — 2026-10-20

Проверялся курс упражнений `01_basics … 20_practiceCalculator`; основной объём замечаний — активный проект `20_practiceCalculator` (ветка `js2`, незакоммиченные правки в `src/viewmodels/viewmodel.ts` и `src/views/view.tsx`).

## Как проверялось

- чтение исходников всех непустых файлов проекта;
- реальный production-сбор: `npx vite build` (результат ниже);
- проверка поведения калькулятора: скрипты с `mathjs` `evaluate()` во временной папке `.qwen/tmp`, удалены после проверки; рабочее дерево не тронуто — `git status` показывает только те два изменённых файла, что были до проверки;
- точечный `tsc --noEmit --strict` (своего `tsconfig.json` в проекте нет, см. пункт 5).

---

## 1. Кнопка `=` ломает выражение — критично

`src/views/view.tsx:146` — `onClick={() => viewModel.addChar("=")}`, то есть `=` просто дописывается в строку. Проверено на вашей же версии mathjs 15.2.0:

```text
"2+2="  -> THROWS SyntaxError: Invalid left hand side of assignment operator = (char 4)
```

Модель перехватывает исключение, возвращает `null`, ViewModel превращает `null` в `""`. Итог: нажатие `=` не вычисляет результат, а **портит ввод** — живое поле `Результат` из вычисленного становится пустым, и следующие нажатия цифр тоже дают пустоту, пока пользователь не удалит `=` кнопкой `remove`.

`=` должна быть отдельным действием, а не символом выражения. Варианты кода — в пункте 12.

## 2. `calculate()` вызывается во время render

`src/views/view.tsx:21` — `Результат: {viewModel.calculate()}`. Это метод с побочными эффектами, вызываемый из JSX:

- `console.log(result)` внутри `calculate()` (`src/viewmodels/viewmodel.ts:19`) пишет в консоль на **каждый** render;
- полное вычисление mathjs происходит при каждом ре-рендере, а не при изменении выражения;
- при включении `<StrictMode>` вызовы будут двойными, и это сразу станет видно в логах.

В MobX для этого есть `computed` — геттер `get result()`, который мемоизируется и пересчитывается только при изменении `calculatedString`. Это и есть аналог связывания свойства (binding) из WPF, а не вызова метода.

## 3. Двойная обработка ошибок: текст `Ошибка` недостижим

`CalculationModel.calculate()` (`src/models/CalculationModel.ts:14-21`) сам обёрнут в `try/catch` и возвращает `null`. Поэтому `catch` в `ViewModel.calculate()` (`src/viewmodels/viewmodel.ts:23-26`) при ошибках вычисления **никогда не выполняется**, и строка `Ошибка` с `console.error` никогда не появляется — пользователь видит пустое поле вместо сообщения. Плюс модель печатает `console.log` при каждой ошибке.

Остался только путь `null -> ""`. Нужно выбрать один слой обработки ошибок.

Проверено, что `evaluate("")` возвращает `undefined`, а не число, — поэтому проверка `result !== null && result !== undefined` в `ViewModel` сейчас корректна; пустое выражение показывается как пустая строка, это ожидаемое поведение.

## 4. mathjs возвращает не только числа, и UI показывает мусор

Результаты проверки на установленных версиях (`mathjs` 15.2.0):

```text
"1/0"        -> Infinity          -> в UI "Infinity"
"0/0"        -> NaN               -> в UI "NaN"
"sqrt(-1)"   -> Complex, "i"      -> в UI "i"
"0.1+0.2"    -> 0.30000000000000004
"1e21+1"     -> 1e+21
"5!"         -> 120               (на клавиатуре кнопки нет, но присвоить выражение можно)
"2++3"       -> 5                 (mathjs терпит двойной оператор)
"x=5"        -> 5                 (присваивание!)
"3kg + 2m"   -> THROWS Error: Units do not match
"2+"         -> THROWS SyntaxError: Unexpected end of expression
```

`CalculationModel.calculate` объявлен как `number | null`, но `evaluate` типизирован широко, и TS это не поймает (пункт 5 — проверки типов нет вообще). Нужен явный guard по типу и конечности значения и округление для показа.

Хорошая новость про безопасность: `Math`, `global`, `process.env`, `1..constructor` внутри `evaluate` недоступны — все дают `Undefined symbol`, так что исполняемого кода через строку ввода нет.

## 5. `tsconfig.json` нет нигде — TypeScript не проверяется

Проверено: рекурсивный поиск `tsconfig*` по `js/react` даёт результаты только внутри `node_modules`. Файлы при этом `.ts`/`.tsx`, а `typescript` (7.0.2) присутствует в `node_modules` как чужая транзитивная зависимость и **не объявлен** в `package.json` проекта.

Следствие: Vite/esbuild просто срезает типы, ни одна ошибка типов не видна. Точечный прогон `tsc --noEmit --strict` по пяти файлам проекта нашёл ровно одну ошибку — и она как про ту же причину:

```text
src/app.tsx:5:8 - error TS2882: Cannot find module or type declarations for side-effect import of './index.css'.
```

То есть первому же настоящему `tsconfig.json` не хватит `types: ["vite/client"]` — и все последующие проверки типов сейчас просто отсутствуют.

## 6. `package.json` `20_practiceCalculator`

```json
"main": "index.js",   // файла нет — проверено
"type": "commonjs",   // приложение ESM, конфиги .mjs
```

- Нет скриптов `build`, `preview`, `type-check`, `test`.
- 5 devDependencies сидят на `"latest"` (строки 25, 34–37, 40: `@chromatic-com/storybook`, `@vitest/browser-playwright`, `@vitest/coverage-v8`, `playwright`, `prop-types`, `vitest`) — установка невоспроизводима, `npm ci` через полгода даст другой граф.
- `@types/mathjs: ^9.4.1` — это **заглушка**: в её `package.json` стоит `"typings": null` и описание «Stub TypeScript definitions entry for mathjs, which provides its own types definitions». mathjs 15 несёт свои типы. Удалить.
- `prop-types` в TS-проекте не нужен.
- Установлены `vitest`, `@vitest/browser-playwright`, `@vitest/coverage-v8`, `playwright`, `@storybook/addon-vitest` (и аддон зарегистрирован в `.storybook/main.mjs`), но: ни одного тест-файла (`*.test.*`/`*.spec.*` — 0 результатов), нет `vitest.config.*`, нет скрипта `test`. Playwright к тому же тянет скачивание браузеров. Либо добавить тесты, либо убрать этот блок.

## 7. Размер бандла

Реальный сбор `npx vite build` (1100 модулей):

```text
dist/index.html                    0.39 kB │ gzip:   0.28 kB
dist/assets/index-N2pcdip0.css     7.43 kB │ gzip:   2.40 kB
dist/assets/index-C1oA4HJh.js    945.03 kB │ gzip: 278.31 kB
(!) Some chunks are larger than 500 kB after minification.
```

945 kB JS для калькулятора — это почти целиком `mathjs`, импортируемый целиком (`import { evaluate } from "mathjs"`).

Проверено, что у mathjs 15.2.0 есть субпакет `./number` (разрешается в `node_modules/mathjs/lib/cjs/number.js`) — это урезанная сборка только под числа: она не отдаёт Complex/BigNumber/Unit, то есть заодно закрывает половину проблем из пункта 4. **Сколько именно килобайт это экономит, я не измерял** — прогон A/B-сборки во временной папке не удалось корректно настроить (пути разрешения не сходились), и после неудачи я её просто удалил. Проверить одной командой после перехода на `"mathjs/number"`: `npx vite build`.

## 8. `CalculationModel` почти полностью мёртв, MVVM формален

- `inputString` присваивается в конструкторе и больше нигде не читается;
- `defaultNumber = 22` и `increment()` (`src/models/CalculationModel.ts:6, 23-26`) — остатки упражнения «счётчик», вызовов нет;
- `ViewModel` создаёт `new CalculationModel("")`, но передаёт в `calculate()` **свою** строку `this.calculatedString`. Состояние модели не используется вообще.

Весь state живёт в ViewModel, модель — обёртка над одной функцией. Два честных варианта: либо модель владеет выражением (тогда `makeAutoObservable` в ней обязателен — см. известную ловушку MobX 7: `makeAutoObservable` во ViewModel не доходит до вложенного класса-модели), либо модель остаётся чистым сервисом-вычислителем без состояния, и тогда `makeAutoObservable` и конструктор-аргумент из неё надо убрать.

## 9. `view.tsx`: 20 копипаст-блоков `<Button>`

Каждый элемент клавиатуры — отдельный JSX-блок с вручную прописанными `row`/`column` и замыканием в `onClick`. Раскладка 4×5 = 20 ячеек заполнена полностью (проверено по координатам, коллизий нет), но добавлять клавишу сейчас значит копировать 7 строк.

Лучше описать клавиши массивом данных и рендерить через `.map()` — не забыть `key`. Классы Tailwind при этом остаются литералами в `button.styles.ts`, так что сканер Tailwind v4 не сломается (ваших динамических классов в массиве не будет, только метки/координаты).

Мелочь: `className=""` у кнопки `remove` — шум, убрать.

## 10. `button.tsx` / `button.styles.tsx`

- Цепочка `if` для выбора стиля (`button.tsx:34-39`) перекрывает самое первое значение: `appliedClassName` инициализируется как `buttonNumberStyle`, а `buttonType === Number` затем проверяется отдельно — первое присваивание фактически мёртвое. Нужна карта `ButtonType -> класс`.
- `ButtonType.Number === 0` — numeric enum с нулевым первым значением; значение по умолчанию `ButtonType.Manage` спасает, но строковый enum безопаснее.
- Проп `disabled` объявлен, но вьюхой никогда не передаётся (используется только в сторибук-историях).
- `<button>` без `type="button"`; сейчас формы нет, но добавится — и кнопки начнут сабмитить.
- `buttonVariants` (`operator`, `ghost`) используется только в историях. `ghost` даёт `bg-transparent`/`text-slate-300`, а базовый класс — `bg-*-500`/`text-black`: конфликтующие утилиты в одном `className`, победитель зависит от порядка генерации Tailwind, а не от порядка конкатенации. Хрупко (в этом проекте уже была история с конкатенацией классов).
- `buttonTypes.ts` — **пустой файл (0 байт)**, при этом в гите. `ButtonType` и `ButtonProps` живут в `button.tsx`. Либо перенести типы в `buttonTypes.ts` (для этого файл и создан), либо удалить его.
- `view.tsx:4-5` импортирует `Button` и `ButtonType` двумя отдельными строками из одного модуля — объединить.

## 11. Мелочи и окружение

- Опечатка: `handeledString` → `handledString` (`viewmodel.ts:34`).
- Имена методов: `addCe` очищает всё выражение, а не текущий ввод (CE = clear entry) → `clearAll`; `removeLast` → `backspace`; `getCalculatedString()` при одноимённом поле — лучше геттер.
- `console.log`/`console.error` в ViewModel и модели — убрать, это слой UI/отладки.
- `index.html`: `<title>Document</title>`, при `lang="en"` интерфейс русскоязычный.
- `app.tsx`: `createRoot(...)` вызывается на уровне модуля, поэтому импорт `app.tsx` имеет побочный эффект (в сторибук-историях это ловушка). Обычно делят на `main.tsx` (точка входа, правка `index.html`) и `app.tsx` (компонент). `<App></App>` → `<App />`.
- Два `<h1>` на странице вместо заголовков — это поле вывода. Лучше `<output>` или `role="status" aria-live="polite"`, чтобы результат озвучивался скринридером.
- Шаблоны из `npx sb init` (`src/stories/Button.jsx`, `Header.jsx`, `Page.jsx`, `Configure.mdx`, `*.css`, `assets/*`) в гите и не относятся к калькулятору — удалить или оставить как справочник, но тогда они навсегда в сайдбаре Storybook.
- `.qwen/` не игнорируется (`git check-ignore` подтверждает) и висит в `git status` как `?? .qwen/`. `debug-storybook.log` при этом уже игнорируется (`*storybook.log`, строка 98 `.gitignore`) — его трогать не надо.
- CI: `.github/workflows/` существует, но пуст.
- В `js/react` 35 каталогов `node_modules` (включая вложенные) — у каждого упражнения свой install; только `20_practiceCalculator` весит 213 МБ. Плюс README нет ни в одном упражнении.

---

## 12. Код для правок (полный, вставлять целиком)

### `src/models/CalculationModel.ts`

```ts
import { evaluate } from "mathjs";

export class CalculationModel {
  evaluate(input: string): string {
    if (input.trim() === "") {
      return "";
    }

    try {
      const value: unknown = evaluate(input);

      if (typeof value !== "number" || !Number.isFinite(value)) {
        return "Ошибка";
      }

      return String(Number(value.toPrecision(12)));
    } catch {
      return "Ошибка";
    }
  }
}
```

Состояния в модели нет — `makeAutoObservable` и `inputString` убраны осознанно. Если решите перенести выражение в модель, верните и конструктор с `makeAutoObservable(this)`, иначе ViewModel не увидит изменений (вложенные классы MobX 7 сам не observable-ит).

### `src/viewmodels/viewmodel.ts`

```ts
import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/CalculationModel";

export class ViewModel {
  calculatedModel: CalculationModel = new CalculationModel();
  calculatedString: string = "";

  constructor() {
    makeAutoObservable(this);
  }

  get result(): string {
    return this.calculatedModel.evaluate(this.calculatedString);
  }

  addChar = (input: string): void => {
    this.calculatedString += input;
  };

  removeLast = (): void => {
    this.calculatedString = this.calculatedString.slice(0, -1);
  };

  addCe = (): void => {
    this.calculatedString = "";
  };

  equals = (): void => {
    const value = this.result;

    if (value !== "" && value !== "Ошибка") {
      this.calculatedString = value;
    }
  };
}
```

`result` — computed: читается как поле, мемоизируется, без `console.log` в render. `equals()` закрепляет посчитанное значение в строку ввода (классическое поведение калькулятора: после `=` можно продолжать вычисление). Если хотите «чистый» `=` без live-превью — тогда `get result()` убрать, ввести observable-поле `result` и заполнять его только в `equals()`.

### `src/views/view.tsx`

```tsx
import React from "react";
import { observer } from "mobx-react-lite";
import { ViewModel } from "../viewmodels/viewmodel";
import { Button, ButtonType } from "../components/ui/buttons/button";

interface CalculationViewProps {
  viewModel: ViewModel;
}

type KeyAction = "append" | "backspace" | "clear" | "equals";

interface Key {
  label: string;
  value: string;
  action: KeyAction;
  buttonType: ButtonType;
  row: number;
  column: number;
}

const KEYS: Key[] = [
  { label: "(", value: "(", action: "append", buttonType: ButtonType.Bracket, row: 1, column: 1 },
  { label: ")", value: ")", action: "append", buttonType: ButtonType.Bracket, row: 1, column: 2 },
  { label: "CE", value: "", action: "clear", buttonType: ButtonType.Manage, row: 1, column: 3 },
  { label: "remove", value: "", action: "backspace", buttonType: ButtonType.Manage, row: 1, column: 4 },
  { label: "7", value: "7", action: "append", buttonType: ButtonType.Number, row: 2, column: 1 },
  { label: "8", value: "8", action: "append", buttonType: ButtonType.Number, row: 2, column: 2 },
  { label: "9", value: "9", action: "append", buttonType: ButtonType.Number, row: 2, column: 3 },
  { label: "/", value: "/", action: "append", buttonType: ButtonType.Operator, row: 2, column: 4 },
  { label: "4", value: "4", action: "append", buttonType: ButtonType.Number, row: 3, column: 1 },
  { label: "5", value: "5", action: "append", buttonType: ButtonType.Number, row: 3, column: 2 },
  { label: "6", value: "6", action: "append", buttonType: ButtonType.Number, row: 3, column: 3 },
  { label: "*", value: "*", action: "append", buttonType: ButtonType.Operator, row: 3, column: 4 },
  { label: "1", value: "1", action: "append", buttonType: ButtonType.Number, row: 4, column: 1 },
  { label: "2", value: "2", action: "append", buttonType: ButtonType.Number, row: 4, column: 2 },
  { label: "3", value: "3", action: "append", buttonType: ButtonType.Number, row: 4, column: 3 },
  { label: "-", value: "-", action: "append", buttonType: ButtonType.Operator, row: 4, column: 4 },
  { label: "0", value: "0", action: "append", buttonType: ButtonType.Number, row: 5, column: 1 },
  { label: ".", value: ".", action: "append", buttonType: ButtonType.Operator, row: 5, column: 2 },
  { label: "=", value: "", action: "equals", buttonType: ButtonType.Operator, row: 5, column: 3 },
  { label: "+", value: "+", action: "append", buttonType: ButtonType.Operator, row: 5, column: 4 },
];

interface KeyProps extends Key {
  viewModel: ViewModel;
}

const KeyButton: React.FC<KeyProps> = ({ viewModel, label, value, action, buttonType, row, column }) => {
  const handleClick = () => {
    if (action === "append") {
      viewModel.addChar(value);
      return;
    }
    if (action === "backspace") {
      viewModel.removeLast();
      return;
    }
    if (action === "clear") {
      viewModel.addCe();
      return;
    }
    viewModel.equals();
  };

  return (
    <Button label={label} buttonType={buttonType} onClick={handleClick} row={row} column={column} />
  );
};

export const CalculatorView: React.FC<CalculationViewProps> = observer(({ viewModel }) => {
  return (
    <div>
      <p className="text-3xl font-bold">Выражение: {viewModel.calculatedString}</p>
      <output className="text-3xl font-bold" aria-live="polite">
        Результат: {viewModel.result}
      </output>
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "repeat(4, 1fr)",
          gridTemplateRows: "repeat(5, 1fr)",
          gap: "10px",
        }}
      >
        {KEYS.map((key) => (
          <KeyButton key={`${key.row}-${key.column}`} viewModel={viewModel} {...key} />
        ))}
      </div>
    </div>
  );
});
```

`import React` оставлен намеренно: `@vitejs/plugin-react` в проекте не установлен (проверено — `node_modules/@vitejs` отсутствует), JSX-трансформ классический.

### `tsconfig.json` (новый файл в корне `20_practiceCalculator`)

```json
{
  "compilerOptions": {
    "target": "ES2022",
    "lib": ["ES2022", "DOM", "DOM.Iterable"],
    "module": "ESNext",
    "moduleResolution": "bundler",
    "jsx": "react-jsx",
    "strict": true,
    "noEmit": true,
    "skipLibCheck": true,
    "isolatedModules": true,
    "types": ["vite/client"]
  },
  "include": ["src", ".storybook"]
}
```

```cmd
npm i -D typescript && npm pkg delete dependencies.@types/mathjs
```

`types: ["vite/client"]` заодно снимает единственную ошибку, которую я нашёл ручным прогоном (`import "./index.css"` без деклараций).

### `package.json` — скрипты (полный блок вместо текущего)

```json
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "type-check": "tsc --noEmit",
    "storybook": "storybook dev -p 6006",
    "build-storybook": "storybook build"
  },
```

Заодно: заменить `"latest"` на конкретные версии, убрать `prop-types`, `"main": "index.js"` и `"type": "commonjs"`, добавить `"private": true`.

### `.gitignore` (корень `SelfEducation`) — добавить

```gitignore
# Qwen Code session scratch
.qwen/
```

---

## 13. Порядок правок

1. `=` и live-результат (пункты 1–3) — это сломанный сценарий, который вы видите в UI каждый раз.
2. Guard над возвратом mathjs + округление (пункт 4).
3. `tsconfig.json` + `type-check` + удаление `@types/mathjs` (пункты 5, 6).
4. `build`/`preview` в скрипты, пины версий вместо `"latest"` (пункт 6).
5. Решение по модели: состояние в модель или чистый сервис (пункт 8).
6. Клавиатура из массива + карта стилей вместо цепочки `if` + `buttonTypes.ts` (пункты 9, 10).
7. Бандл: `mathjs/number` и замер `npx vite build` (пункт 7).
8. Тесты: либо `vitest.config.*` + 3–4 теста на `addChar/removeLast/evaluate`, либо удалить vitest/playwright-блок (пункт 6).
9. Мелочи (пункт 11), `.qwen/` в `.gitignore`, шаблонные истории Storybook.

## 14. Что не проверялось

- Точный вклад `mathjs` в 945 kB и экономия от `mathjs/number` — не измерялось (неудачная A/B-сборка во временной папке, удалена).
- Тесты и линтеры в проекте отсутствуют, запускать было нечего.
- Поведение UI в браузере не проверялось (Browser Use на этой машине не работает); выводы по `=` и по `Ошибка` сделаны по коду и по прямым вызовам `evaluate` в Node.
