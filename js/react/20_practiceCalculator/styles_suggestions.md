# Ревизия стилей и структуры `20_practiceCalculator` — 2026-10-22

Проверялся весь активный проект (ветка `js2`): `src/components/ui/buttons/*`, `src/views/view.tsx`, `src/viewmodels/viewmodel.ts`, `src/models/CalculationModel.ts`, `src/app.tsx`, плюс конфиги `tsconfig.json`, `vite.config.mjs`, `package.json`, `.storybook/*`, `index.html`.

## Как проверялось

- `npm run type-check` (`tsc --noEmit`) — было 1 ошибка, сейчас 0 (см. пункт 0);
- `npx tsc --noEmit --noUnusedLocals --noUnusedParameters` — 2 ошибки, обе мёртвые импорты (пункты 1 и 3);
- `npm run build-storybook` — exit 0, `button.stories-*.js` собирается;
- `npx vite build --outDir .qwen/tmp/stylercheck --minify false` — сборка в **временную** папку, чтобы прочитать emitted CSS; `npm run build` (minified) тоже прогонялся отдельно, `dist/` осталась;
- фактический transform модуля через dev-сервер: `npx vite --port 5199` + `curl http://localhost:5199/.qwen/tmp/probe/probe.tsx?import` —tsx-файл **без** `import React` вернулся как `import ... from "/node_modules/.vite/deps/react_jsx-dev-runtime.js"; const jsxDEV = ...`;
- рабочее дерево источников не тронуто, кроме файла из пункта 0; все временные скрипты `.qwen/tmp` удалены.

> Не проверено визуально: как именно выглядят кнопки в браузере. Browser automation на этой машине не работает, поэтому все выводы по CSS сделаны по emitted-файлам сборки, а не по скриншоту.

## 0. Что уже переписано за эту сессию

`src/components/ui/buttons/button.stories.tsx` — по вашему запросу («fix the file and add all button types»). Было: `className: buttonBase` → **TS2322**, потому что `buttonBase` это `string[]`, а проп `className` объявлен как `string`; и ни один `ButtonType` не использовался. Стало: по стори на каждый тип + `Disabled`, `CustomClassName`, `AllTypes`. После правки `type-check` и `build-storybook` зелёные.

Дальше — что стоит сделать вам. Порядок в конце, в чек-листе.

## 1. Мёртвый импорт из `mathjs` в представлении

`src/views/view.tsx:6`:

```ts
import { column } from "mathjs";
```

Нигде не используется (`tsc --noUnusedLocals` прямо это говорит: TS6133). Похоже на автодополнение IDE — имя сталкивается с пропом `column={...}` у кнопок, поэтому читатель теряет время, решая, что `column` откуда-то берётся. Плюс это лишняя причина тянуть mathjs в граф модуля view. Удалить строку.

## 2. `if`-цепочка в `Button` даёт неокрашенную кнопку без ошибки

`src/components/ui/buttons/button.tsx:35-41`:

```ts
let appliedClassName;
if (buttonType === ButtonType.Manage) appliedClassName = buttonManageStyle;
if (buttonType === ButtonType.Number) appliedClassName = buttonNumberStyle;
...
```

`appliedClassName` без типа и без `else`. Добавьте пятое значение в `ButtonType` (или опечатку в имени) — и `className` соберётся из `[undefined, ""]`, то есть кнопка отрендерится **без фона и без сетки**, без ошибки компиляции и без сообщения в консоли. Это ровно тот класс «тихих» CSS-сбоев, который у вас повторяется.

Замена на `Record` превращает пропуск в ошибку компиляции (полный файл — пункт 6).

## 3. `buttonType` не должен иметь значения по умолчанию

`src/components/ui/buttons/button.tsx:33` — `buttonType = ButtonType.Manage`. Забыли проп → зелёная кнопка вместо ошибки. Все 19 мест в `view.tsx` и все стори проп передают, так что сделать его обязательным безопасно:

```ts
// было
buttonType?: ButtonType;
// станет
buttonType: ButtonType;
```

Проверено: после объявления обязательным `tsc --noEmit` на текущем дереве не падает, потому что значение передаётся везде.

## 4. Строка `"Ошибка"` используется как управляющий сигнал

`src/models/CalculationModel.ts` возвращает текст для пользователя, а `src/viewmodels/viewmodel.ts:28-34` потом сравнивает с этой строкой:

```ts
if (value !== "" && value !== "Ошибка") {
  this.calculatedString = value;
}
```

Две проблемы: (а) слой модели знает про язык интерфейса; (б) перевод или опечатка в тексте молча ломают логику — сравнение перестанет срабатывать, и `equals()` начнёт записывать в выражение текст ошибки. Правильно — типизированный результат (код в пункте 12).

## 5. `buttonVariants` — мёртвый код, и он проигрывает каскад

`src/components/ui/buttons/button.styles.tsx:23-26`:

```ts
export const buttonVariants = {
  operator: "bg-amber-500 hover:bg-amber-600",
  ghost: "bg-transparent text-slate-300",
};
```

После переписывания стори (пункт 0) на этот экспорт **ни одна ссылка в проекте не осталась** (`grep` находит только само объявление). Он же из `button.tsx` недоступен в принципе: там цвета берутся из `buttonNumber/Operator/Manage/Bracket`.

Отдельно — почему он и не работал. `Button` склеивает `[appliedClassName, className]`, то есть на элементе оказывались два разных utility на одно свойство `background-color`. Проверено по emitted CSS (`.qwen/tmp/stylercheck/assets/index-Dkog7rtz.css`, нормализованный текст):

```text
.bg-amber-500   -> смещение 5583
.bg-purple-500  -> смещение 5746   (позже => выигрывает)
```

Порядок классов в атрибуте `class` не важен, важен порядок правил в файле Tailwind. То есть `className={buttonVariants.operator}` поверх `buttonType={ButtonType.Operator}` давал **фиолетовый**, а не янтарный — без всякой ошибки. Вдобавок `operator` в этом объекте означает янтарный, а `buttonOperator` — фиолетовый: одно слово, два цвета. Удалить `buttonVariants` целиком.

Полезный побочный факт из той же проверки: `hover`-правила обёрнуты в `@media(hover:hover)`, а `.disabled\:opacity-40:disabled{opacity:.4}` присутствует — `disabled`-состояние работает.

## 6. `button.styles.tsx`: смешаны три стиля экспорта и неверное расширение

Сейчас один файл отдаёт: `buttonBase` как `string[]`, четыре варианта как `string`, `buttonVariants` как объект строк без базовых классов. Именно эта несогласованность и уронила стори в начале сессии (`string[]` в `className: string`).

Второе: в файле **нет JSX**, но расширение `.tsx`. Tailwind v4 сканирует и `.ts`, и `.tsx` одинаково, так что переименование безопасно:

```bat
git mv src/components/ui/buttons/button.styles.tsx src/components/ui/buttons/button.styles.ts
```

Полный вариант файла:

```ts
// src/components/ui/buttons/button.styles.ts
const buttonBase = [
  "h-full",
  "w-full",
  "cursor-pointer",
  "rounded-md",
  "px-4",
  "py-2",
  "text-lg",
  "text-black",
  "transition-colors",
  "duration-300",
  "col-start-(--col)",
  "row-start-(--row)",
  "disabled:cursor-not-allowed",
  "disabled:opacity-40",
];

const withColor = (color: string) => [...buttonBase, color].join(" ");

export const buttonNumber = withColor("bg-red-500");
export const buttonOperator = withColor("bg-purple-500");
export const buttonManage = withColor("bg-green-500");
export const buttonBracket = withColor("bg-blue-500");
```

`buttonBase` больше не экспортируется — извне он не нужен, а его экспорт и был источником ошибки с типом. Цвет остаётся полным литералом: `bg-${color}` интерполяцию v4-сканер не увидит.

И полный `button.tsx` к нему в пару:

```tsx
// src/components/ui/buttons/button.tsx
import type { CSSProperties } from "react";
import {
  buttonBracket,
  buttonManage,
  buttonNumber,
  buttonOperator,
} from "./button.styles";

export enum ButtonType {
  Number,
  Operator,
  Manage,
  Bracket,
}

export interface ButtonProps {
  label: string;
  onClick: () => void;
  buttonType: ButtonType;
  disabled?: boolean;
  row?: number;
  column?: number;
  className?: string;
}

const buttonTypeStyles: Record<ButtonType, string> = {
  [ButtonType.Number]: buttonNumber,
  [ButtonType.Operator]: buttonOperator,
  [ButtonType.Manage]: buttonManage,
  [ButtonType.Bracket]: buttonBracket,
};

export function Button({
  label,
  onClick,
  buttonType,
  disabled = false,
  row = 1,
  column = 1,
  className = "",
}: ButtonProps) {
  const gridPosition = { "--col": column, "--row": row } as CSSProperties;

  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      className={[buttonTypeStyles[buttonType], className].filter(Boolean).join(" ")}
      style={gridPosition}
    >
      {label}
    </button>
  );
}
```

`type="button"` — защита от отправки формы, если кнопка окажется внутри `<form>`; сейчас форм нет, но цена нулевая. `ButtonProps` стоит экспортировать: сейчас он локальный, и стори/представления не могут описать пропы типом.

## 7. `import React` больше не обязателен (важно для новых файлов)

Появился `tsconfig.json` с `"jsx": "react-jsx"`, и Vite его уважает — см. «Как проверялось»: JSX компилируется в `jsxDEV` из `react/jsx-dev-runtime`. Значит правило «в каждом файле с JSX нужен `import React`» больше не действует; React нужен только там, где обращаются к пространству имён (`React.FC`, `React.CSSProperties` — в текущем `button.tsx` и `view.tsx`).

`tsc --noUnusedLocals` подтверждает: `src/app.tsx:1` — `React` не используется вовсе. Если включаете флаг (пункт 9), импорт нужно убрать, иначе будет TS6133.

Но: `@vitejs/plugin-react` по-прежнему нет (`vite.config.mjs` подключает только `@tailwindcss/vite`), поэтому Fast Refresh не работает — правка в `.tsx` перезагружает страницу целиком. Это не ошибка, просто перестаньте ждать hot reload.

## 8. `view.tsx`: 19 копий одного и того же блока

`src/views/view.tsx` — 168 строк, из них ~140 на 19 однотипных `<Button>`, где меняются только `label`, тип, `column`, `row` и символ. Единственная полезная информация — раскладка клавиатуры, и она размазана по повторяющемуся коду.

Переносить ли `column`/`row` в Tailwind-классы — **нет**: `col-start-${n}` сканер не увидит. Текущий приём с `--col`/`--row` и литеральными `col-start-(--col)` корректен, поэтому данные из массива дойдут до CSS без изменений.

```tsx
// src/views/view.tsx
import type { CSSProperties } from "react";
import { observer } from "mobx-react-lite";
import { Button, ButtonType } from "../components/ui/buttons/button";
import type { ViewModel } from "../viewmodels/viewmodel";

interface CalculationViewProps {
  viewModel: ViewModel;
}

interface Key {
  label: string;
  buttonType: ButtonType;
  column: number;
  row: number;
  onPress: (viewModel: ViewModel) => void;
}

const append = (value: string) => (viewModel: ViewModel) => viewModel.addChar(value);

const clear = (viewModel: ViewModel) => viewModel.clear();
const removeLast = (viewModel: ViewModel) => viewModel.removeLast();

const keypadStyle: CSSProperties = {
  display: "grid",
  gridTemplateColumns: "repeat(4, 1fr)",
  gridTemplateRows: "repeat(5, 1fr)",
  gap: "10px",
};

const keys: Key[] = [
  { label: "(", buttonType: ButtonType.Bracket, column: 1, row: 1, onPress: append("(") },
  { label: ")", buttonType: ButtonType.Bracket, column: 2, row: 1, onPress: append(")") },
  { label: "CE", buttonType: ButtonType.Manage, column: 3, row: 1, onPress: clear },
  { label: "remove", buttonType: ButtonType.Manage, column: 4, row: 1, onPress: removeLast },

  { label: "7", buttonType: ButtonType.Number, column: 1, row: 2, onPress: append("7") },
  { label: "8", buttonType: ButtonType.Number, column: 2, row: 2, onPress: append("8") },
  { label: "9", buttonType: ButtonType.Number, column: 3, row: 2, onPress: append("9") },
  { label: "/", buttonType: ButtonType.Operator, column: 4, row: 2, onPress: append("/") },

  { label: "4", buttonType: ButtonType.Number, column: 1, row: 3, onPress: append("4") },
  { label: "5", buttonType: ButtonType.Number, column: 2, row: 3, onPress: append("5") },
  { label: "6", buttonType: ButtonType.Number, column: 3, row: 3, onPress: append("6") },
  { label: "*", buttonType: ButtonType.Operator, column: 4, row: 3, onPress: append("*") },

  { label: "1", buttonType: ButtonType.Number, column: 1, row: 4, onPress: append("1") },
  { label: "2", buttonType: ButtonType.Number, column: 2, row: 4, onPress: append("2") },
  { label: "3", buttonType: ButtonType.Number, column: 3, row: 4, onPress: append("3") },
  { label: "-", buttonType: ButtonType.Operator, column: 4, row: 4, onPress: append("-") },

  { label: "0", buttonType: ButtonType.Number, column: 1, row: 5, onPress: append("0") },
  { label: ".", buttonType: ButtonType.Operator, column: 2, row: 5, onPress: append(".") },
  { label: "+", buttonType: ButtonType.Operator, column: 4, row: 5, onPress: append("+") },
];

// Обёртка observer() перерисовывает представление при изменении наблюдаемых,
// которые прочитались при рендере: здесь это calculatedString (через result и напрямую).
export const CalculatorView = observer(function CalculatorView({
  viewModel,
}: CalculationViewProps) {
  return (
    <div>
      <p className="text-3xl font-bold">Выражение: {viewModel.calculatedString}</p>
      <output className="text-3xl font-bold" aria-live="polite">
        Результат: {viewModel.result}
      </output>
      <div style={keypadStyle}>
        {keys.map((key) => (
          <Button
            key={key.label}
            label={key.label}
            buttonType={key.buttonType}
            column={key.column}
            row={key.row}
            onClick={() => key.onPress(viewModel)}
          />
        ))}
      </div>
    </div>
  );
});
```

Что попутно уходит: лишний `<>...</>` вокруг одного `<div>`, `className=""` у `remove` (это no-op), два `<h1>` для строк вывода (заголовок тут не по смыслу, а preflight и так обнулил размер, поэтому `text-3xl` визуально ничего не меняет), мёртвый импорт из пункта 1, и устаревший комментарий `view.tsx:12` — в нём упоминается `viewModel.count`, поля с таким именем в ViewModel уже нет.

Про раскладку: 19 кнопок на сетке 4×5, свободна одна ячейка — `row 5, column 3`. Это ровно место для `=`, см. пункт 12.

## 9. `tsconfig.json`: включить флаги, которые ловят пункты 1 и 3

Файл сейчас **не отслеживается git** (`?? tsconfig.json`), хотя `npm run type-check` — объявленный скрипт: конфиг нужно закоммитить. В `include` есть `".storybook"`, но там только `.mjs`, а `allowJs` не включён — то есть строка ни на что не влияет. Полезное дополнение:

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
    "noUnusedLocals": true,
    "noUnusedParameters": true,
    "noFallthroughCasesInSwitch": true,
    "types": ["vite/client"]
  },
  "include": ["src"]
}
```

`noUnusedLocals` сегодня даёт ровно два падения: `app.tsx:1` (React) и `view.tsx:6` (column). Оба убираются пунктами 1 и 7 — после этого флаг можно оставить навсегда, и следующие мёртвые импорты вылезут сами.

## 10. Тестовая инфраструктура заявлена, но не подключена

В `package.json` стоят `vitest`, `@vitest/coverage-v8`, `@vitest/browser-playwright`, `playwright`, `@testing-library/*`, и `@storybook/addon-vitest` зарегистрирован в `.storybook/main.mjs:12`. При этом:

- файла `vitest.config.*` в проекте нет;
- в `src` нет ни одного `*.test.*` / `*.spec.*`;
- в `scripts` нет команды `test`.

То есть аддон включён, но запускать ему нечего — в сборку на это время уходят `axe-*.js` (676 kB) и `blocks-*.js` (852 kB). Либо добавьте `vitest.config.ts` и `"test": "vitest run"` (самый дешёвый и полезный старт — тесты на `CalculationModel.evaluate` и `ViewModel`: чистые функции, DOM не нужен), либо снимите аддон и зависимости.

Отдельно: `"vitest": "latest"` — единственная незапиненная зависимость в файле; соседние `@vitest/*` стоят жёстко на `5.0.5`. `latest` тихо меняет версию при каждой чистой установке, лучше привести к `5.0.5`.

## 11. Остальное по мелочи

- **`src/stories/`** — leftover от `storybook init` (`Button.jsx`, `Header.jsx`, `Page.jsx`, `Configure.mdx`, три CSS, 12 картинок). `main.mjs:6` подхватывает их тем же глобом, поэтому в сайдбаре есть второй «Button», **не ваш**: легко проверить не тот компонент. Удалить папку либо сузить глоб до `"../src/components/**/*.stories.@(js|jsx|mjs|ts|tsx)"`.
- **`.storybook/preview.mjs:9-10`** — у `color` и `date` по 7 пробелов вместо 8, у соседей 6.
- **`index.html`** — `<title>Document</title>` (заготовка), `lang="en"` при русском тексте интерфейса, отступы в 4 пробела против 2 в остальном проекте.
- **`.gitignore`** — `.qwen/` не игнорируется (`git check-ignore` по `node_modules` и `*storybook.log` находит правила, по `.qwen/tmp` — нет), поэтому временные файлы периодически лезут в `git status`. Добавьте строку `.qwen/`.
- **Именование файлов и классов.** `models/CalculationModel.ts` — PascalCase, а `views/view.tsx`, `viewmodels/viewmodel.ts`, `button.styles.tsx` — нижний регистр; `ViewModel` живёт в `viewmodel.ts`, `CalculatorView` — в `view.tsx`. Правило «имя файла = имя экспортируемого символа» снимает вопрос, где что лежит; менять лучше одним проходом, пока файлов мало.
- **Язык комментариев.** `view.tsx:12` и `app.tsx:6` по-русски, а новые комментарии в стори — по-английски. Выберите один (в этом проекте исторически русский) — иначе получится третий источник правды.
- **`label="remove"`** шире остальных подписей в ячейке той же ширины; `←` или `⌫` короче и читается как клавиша.

## 12. `=` и `equals()` — недостиженный код

`src/viewmodels/viewmodel.ts:28` объявляет `equals()`, но в `view.tsx` нет кнопки, которая её вызывает (проверено `grep` по `src`: единственный вызов — само объявление). Сейчас результат виден только как живое значение `result`, и зафиксировать его нельзя.

Это же чинит пункт 4. Модель перестаёт возвращать текст ошибки:

```ts
// src/models/CalculationModel.ts
import { evaluate as mathEvaluate } from "mathjs";

export type EvaluationResult =
  | { ok: true; display: string }
  | { ok: false; reason: "empty" | "invalid" | "not-a-number" };

export class CalculationModel {
  evaluate(input: string): EvaluationResult {
    if (input.trim() === "") {
      return { ok: false, reason: "empty" };
    }
    try {
      const value: unknown = mathEvaluate(input);
      if (typeof value !== "number" || !Number.isFinite(value)) {
        return { ok: false, reason: "not-a-number" };
      }
      return { ok: true, display: String(Number(value.toPrecision(12))) };
    } catch {
      return { ok: false, reason: "invalid" };
    }
  }
}
```

```ts
// src/viewmodels/viewmodel.ts
import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/CalculationModel";

const ERROR_TEXT = "Ошибка";

export class ViewModel {
  calculatedModel: CalculationModel = new CalculationModel();
  calculatedString: string = "";

  constructor() {
    makeAutoObservable(this);
  }

  get result(): string {
    const evaluation = this.calculatedModel.evaluate(this.calculatedString);
    return evaluation.ok ? evaluation.display : ERROR_TEXT;
  }

  addChar = (input: string): void => {
    this.calculatedString += input;
  };

  removeLast = (): void => {
    this.calculatedString = this.calculatedString.slice(0, -1);
  };

  clear = (): void => {
    this.calculatedString = "";
  };

  equals = (): void => {
    const evaluation = this.calculatedModel.evaluate(this.calculatedString);
    if (evaluation.ok) {
      this.calculatedString = evaluation.display;
    }
  };
}
```

Локализация теперь у ViewModel (это слой представления), а `equals()` смотрит на `ok`, а не на строку. В `view.tsx` из пункта 8 добавьте одну запись в `keys` — в свободную ячейку `row 5, column 3`:

```tsx
  { label: "=", buttonType: ButtonType.Manage, column: 3, row: 5, onPress: equals },
```

и рядом helper `const equals = (viewModel: ViewModel) => viewModel.equals();`.

Не переносите `=` в `addChar("=")`: mathjs 15.2.0 на строке вида `2+2=` бросает `SyntaxError: Invalid left hand side of assignment operator =` (проверялось на вашей же версии), то есть символ испортит выражение вместо вычисления.

## 13. Вес сборки

`npm run build` с minification — `dist/assets/index-*.js` порядка 945 kB (278 kB gzip) при CSS в 9–12 kB; почти всё — mathjs, подключённый через `import { evaluate } from "mathjs"`. Vite на это честно ругается `(!) Some chunks are larger than 500 kB`.

mathjs 15.2.0 в вашей установке отдаёт подпуть `./number` (проверено в `node_modules/mathjs/package.json`) — это number-only сборка, и она же убирает Complex/Unit/BigNumber на выходе. Экономия в kB **не измерена**: замер был бы честным только после смены импорта, `npx vite build` и сравнения `dist/assets/index-*.js`.

## Чек-лист по порядку

1. Удалить `import { column } from "mathjs"` (`view.tsx:6`) — 5 секунд, чистый мёртвый код.
2. Выкинуть `buttonVariants` (`button.styles.tsx:23-26`) — он никем не используется и не мог сработать.
3. `button.styles.tsx` → `withColor` + неэкспортируемый `buttonBase`, переименовать в `.ts`; `button.tsx` → `Record<ButtonType, string>` + обязательный `buttonType`. Проверить: `npm run type-check`.
4. Закоммитить `tsconfig.json` и добавить в него `noUnusedLocals`/`noUnusedParameters`; убрать `import React` из `app.tsx`. Проверить: `npm run type-check`.
5. `view.tsx` на `keys[]` (пункт 8) — biggest readability win. Проверить: `npm run build` + `npm run build-storybook`.
6. Типизированный результат `evaluate` + кнопка `=` в ячейку (5, 3) (пункт 12).
7. Решение по тестам: `vitest.config.ts` + тесты на модель/вьюмодель, либо снять `@storybook/addon-vitest` и тестовые зависимости.
8. Мелочи из пунктов 11, затем отдельно — `mathjs/number` с замером.

Пункты 1–4 не меняют поведение калькулятора, только убирают тихие способы всё сломать. Пункты 5–6 меняют код представления и логику — после них стоит глазами проверить Storybook (`UI/CalculatorButton`) и четыре цвета кнопок.
