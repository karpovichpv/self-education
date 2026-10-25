# Ревизия проекта `20_practiceCalculator` — 2026-10-25

Второй проход по проекту. Ветка `js2`, незакоммиченные правки трогались только на чтение. Прошлый отчёт (`js/react/suggestions.md` от 2026-10-20) удалён из рабочего дерева, `styles_suggestions.md` обрезан до 0 байт — см. пункт 10.

## Как проверялось

Реально прогнанные команды (все — в этой папке проекта):

- `npm run type-check` → **падает**, exit 1 (текст ошибки в пункте 1);
- `npx tsc --noEmit -p .qwen\tmp\check\tsconfig.json` — то же самое на копии `src` во временной папке: до правки exit 1 с той же ошибкой, после правки `meta.args` → **exit 0** (правка проверена);
- `npx vite build` → exit 0, 1105 модулей, JS 945,24 kB (gzip 278,21 kB), CSS 8,14 kB;
- `npx vite build .qwen/tmp/check --outDir dist-mathjs` и `--outDir dist-number` → A/B-замер `mathjs` vs `mathjs/number` (пункт 7): **945,24 kB → 654,22 kB**;
- `npm run build-storybook` → exit 0, с предупреждением `No story files found for the specified pattern: src\**\*.mdx`;
- поведение `ViewModel` прогнано в Node: `npx esbuild .qwen\tmp\probe\run.ts --bundle --platform=node --format=cjs --minify` + `node .qwen\tmp\probe\run.cjs` (пункт 4);
- поведение `mathjs` прогнано в Node на установленной 15.2.0 для обоих входов (пункт 7);
- `git ls-files -s`, `git config core.ignorecase` → `true`, `git -c core.ignorecase=false status --porcelain`, `git ls-files --error-unmatch src\components\ui\Buttons\Button.styles.ts` → **не в индексе** (пункт 2);
- `git check-ignore -v .qwen debug-storybook.log dist storybook-static` → игнорируются (`.gitignore:1 .qwen/`, `:100 *storybook.log`, `:71 dist`, `:101 storybook-static`);
- разбор сгенерированного CSS: `.qwen\tmp\cssseq.cjs` по `dist\assets\index-*.css` (пункт 6);
- чтение всех непустых исходников и конфигов.

Рабочие файлы проекта **не изменены**. Временные папки `.qwen\tmp\check`, `.qwen\tmp\probe`, `.qwen\tmp\mathcheck` удалены после проверки. Предупреждаю честно: прогоны `vite build` и `build-storybook` перезаписали `dist\` и `storybook-static\` — это gitignore-нутые артефакты сборки, они пересоздаются вашими же скриптами.

---

## 1. `npm run type-check` красный — один баг в сторибук-файле

```text
src/components/ui/Buttons/Button.stories.tsx(71,14): error TS2322:
  Property 'args' is missing in type '{ render: (...) => Element; }'
  but required in type '{ args: { ...; buttonType: ButtonType; } }'.
```

Строка 71 — это `export const AllTypes: Story = {`. Причина: в `meta.args` нет **обязательного** пропа `buttonType`, поэтому тип `StoryObj<typeof meta>` начинает требовать `args` у каждой истории. У `AllTypes` есть только `render` → ошибка.

Проверено: в копии проекта добавление `buttonType` в `meta.args` снимает ошибку полностью (`tsc` exit 0), при этом остальные истории менять не нужно.

`src/components/ui/Buttons/Button.stories.tsx` — блок `meta` целиком:

```tsx
const meta = {
  title: "Components/Button",
  component: Button,
  args: {
    label: "5",
    buttonType: ButtonType.Number,
    row: 3,
    column: 2,
    onClick: () => {},
  },
  decorators: [
    (Story) => (
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "repeat(4, 80px)",
          gridTemplateRows: "repeat(5, 48px)",
          gap: "10px",
        }}
      >
        <Story />
      </div>
    ),
  ],
} satisfies Meta<typeof Button>;
```

Дублирование сетки грида: раскладка клавиатуры живёт в двух местах — `keypadStyle` в `src/Views/View.tsx:25` (`repeat(4, 1fr) / repeat(5, 1fr)`) и декоратор в историях (`repeat(4, 80px) / repeat(5, 48px)`). Значения уже разошлись. Номинальный размер ячейки — это пропсы (`cellWidth`/`cellHeight`) или один общий `keypadStyle`, импортируемый из `button.styles.ts`.

## 2. Критично для репозитория: git и диск разошлись по регистру, а `Button.styles.ts` вообще не в гите

Проверено по индексу, а не по рабочему дереву — то есть по тому, что реально попадёт в клон:

| `git ls-files` (индекс)                           | на диске (`dir /b /s src`)                                   |
| ------------------------------------------------- | ------------------------------------------------------------ |
| `src/app.tsx`                                     | `src/App.tsx`                                                |
| `src/models/CalculationModel.ts`                  | `src/Models/CalculationModel.ts`                             |
| `src/viewmodels/viewmodel.ts`                     | `src/Viewmodels/Viewmodel.ts`                                |
| `src/views/view.tsx`                              | `src/Views/View.tsx`                                         |
| `src/components/ui/buttons/button.tsx`            | `src/components/ui/Buttons/Button.tsx`                       |
| `src/components/ui/buttons/button.styles.tsx` (D) | `src/components/ui/Buttons/Button.styles.ts` — **untracked** |

`git config core.ignorecase` = `true`, поэтому переименование только в регистре Windows-git не увидел: `git -c core.ignorecase=false status --porcelain` показывает каждую такую пару как ` D` (удалён старый) + `??` (новый не в индексе), а обычный `git add -A` регистр не фиксирует.

Что это значит на практике. В клоне (Linux/CI/Docker) окажется **нижнепапковый** вариант из индекса, а код импортирует папковый:

- `src/app.tsx` содержит `import { ViewModel } from "./Viewmodels/Viewmodel"` → папки `Viewmodels` нет, есть `viewmodels` → ошибка разрешения модуля;
- `src/views/view.tsx` импортирует `../components/ui/Buttons/Button` → нет папки `Buttons`;
- файла `Button.styles.ts` в клоне нет вовсе (`??`) → `Button.tsx` не соберётся даже на Windows-машине коллеги;
- в `index.html` подключён `./src/app.tsx` (строчные) — на случайной ФС это ломает точку входа, хотя сейчас совпадает с индексом.

То есть **собранный здесь проект не собирается из репозитория**. Что `npm run build` локально зелёный — не аргумент: Windows ФС не различает регистр.

Восстановление (индекс mutations — выполняйте сами, рабочее дерево `git rm --cached` не трогает):

```cmd
cd /d C:\other\SelfEducation\js\react\20_practiceCalculator
git -c core.ignorecase=false rm -r --cached src
git add src
git status --short
```

После этого в индексе появится ровно тот регистр, который на диске. Если примете вариант из пункта 3 (строчные имена), сначала переименуйте файлы/папки, потом выполните те же три команды. И отдельно: `Button.styles.ts` нужно закоммитить в любом случае.

## 3. Вопрос «всё ли названо по best practices» — нет, и вот именно что

Основной нормой для React/Vite-проектов считается **kebab-case для файлов и папок** и **PascalCase для идентификаторов** (`class`, `interface`, компонента). PascalCase-папки (`Models/`, `Components/`) — легитимный house style из Angular/Next.js, но он должен быть последовательен во всём дереве. Сейчас смешаны оба стиля внутри одного пути: `src/components/ui` (строчные) + `Buttons` (Pascal) + `Models`/`Viewmodels`/`Views` (Pascal). Плюс регистровая проблема из пункта 2 — это прямое следствие выбора PascalCase на Windows.

Отдельно важное отличие от C#/WPF, где выComing из: в TypeScript **имя файла не обязано совпадать с именем класса/типа**. `Viewmodel.ts` с `class ViewModel` компилируется и ничему не противоречит технически — но человек, ищущий `ViewModel`, по имени файла его не найдёт. Правило «file name = type name» из Visual Studio сюда не переносится; здесь главное — единообразие стиля имён файлов.

Что не по конвенции и чем это чревато:

| Сейчас                                                                           | Как принято                                                                  | Почему                                                                                                                                                                       |
| -------------------------------------------------------------------------------- | ---------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `src/Models/`, `src/Viewmodels/`, `src/Views/`, `src/components/ui/Buttons/`     | `src/models/`, `src/view-models/`, `src/views/`, `src/components/ui/button/` | kebab-case; папка компонента — в единственном числе (namespace одного компонента), как в shadcn/MUI                                                                          |
| `Viewmodels`                                                                     | `view-models` (или `ViewModels`, если оставлять Pascal)                      | слово пишется как `ViewModel`: `View-models`/`ViewModels`; `Viewmodels` не совпадает ни с одним из двух стилей                                                               |
| `Viewmodel.ts` (класс `ViewModel`)                                               | `calculator-view-model.ts` (класс `CalculatorViewModel`)                     | `ViewModel` — слишком общо; в MVVM обычно имя = сущность экрана                                                                                                              |
| `Views/View.tsx` (экспорт `CalculatorView`, пропсы `CalculationViewProps`)       | `views/calculator-view.tsx` + `CalculatorViewProps`                          | три корня для одной сущности: `Calculator`, `Calculation`, `View` — имя файла ≠ имя компонента ≠ имя пропсов                                                                 |
| `App.tsx` = компонент + `createRoot` на уровне модуля                            | `main.tsx` (точка входа, побочные эффекты) + `App.tsx` (только компонент)    | конвенция Vite; импорт `App.tsx` сейчас имеет побочный эффект                                                                                                                |
| `<App></App>`                                                                    | `<App />`                                                                    | самозакрывающийся тег для children-less элемента                                                                                                                             |
| `calculatedString`                                                               | `expression` (или `input`)                                                   | поля содержит **ввод**, а не посчитанное значение; имя врёт                                                                                                                  |
| `calculatedModel`                                                                | `evaluator` / `calculator`                                                   | то же: stateless-сервис, а не «посчитанная модель»                                                                                                                           |
| `addChar(value: string)`                                                         | `append(char)` или `append(value)`                                           | метод принимает строку, не символ; и `addChar` vs `onPress: append` ввью — два имени для одной операции                                                                      |
| `const clear` / `const removeLast` рядом с `viewModel.clear()` / `.removeLast()` | `pressClear` / `pressBackspace` (или один тип `onPress`-фабрика)             | имена совпадают с методами VM两 строками ниже и читаются как рекурсия                                                                                                        |
| `append("(")` — фабрика, `clear` — готовый обработчик                            | одинаковая форма для всех строк таблицы                                      | в одном массиве `keys` два разных контракта: `onPress: append("(")` против `onPress: clear`; при добавлении кнопки легко написать `onPress: append` и получить тихую поломку |
| `interface Key`, `key={key.label}`                                               | `interface KeypadButton`, `key={button.id}`                                  | `key` рядом с React-пропсом `key` — постоянная путаница; плюс `key={key.label}` завязан на уникальность метки                                                                |
| `ButtonType.Number`                                                              | `ButtonType.Digit`                                                           | `Number` — глобал JS; в историях уже пришлось писать `NumberKey as Number` с комментарием, чтобы его не затенять                                                             |
| `ButtonType.Manage`                                                              | `ButtonType.Action` (CE, ⌫)                                                  | «Manage» — не существительное для этих клавиш                                                                                                                                |
| `buttonUnset` (`Button.styles.ts:24`)                                            | удалить                                                                      | grep по `src` не нашёл ни одного использования                                                                                                                               |
| `reason` в `EvaluationResult`                                                    | использовать или убрать см. пункт 6                                          | мёртвая часть union-типа                                                                                                                                                     |
| `title: "UI/CalculatorButton"`                                                   | `"Components/Button"`                                                        | имя истории ≠ имя компонента и ≠ путь в `components/ui/…`                                                                                                                    |
| `20_practiceCalculator`                                                          | `20_practice_calculator`                                                     | соседние упражнения курса в `NN_snake_case` (`01_basics`); `package.json → "name": "20_practicecalculator"` — тут всё правильно, npm требует lowercase                       |
| этот отчёт: `2026-10-25 suggestions.md`                                          | `2026-10-25-suggestions.md`                                                  | пробел в имени файла — лишние кавычки в CLI, уродливые URL, `git` показывает путь в кавычках                                                                                 |

Рекомендуемое дерево (после этого пункта регистровая проблема из пункта 2 исчезает сама, потому что файлы начинают называться как в индексе):

```text
src/
├── index.css
├── main.tsx
├── App.tsx
├── models/
│   └── calculation-model.ts        // class CalculationModel
├── view-models/
│   └── calculator-view-model.ts    // class CalculatorViewModel
├── views/
│   └── calculator-view.tsx         // CalculatorView, CalculatorViewProps
└── components/
    └── ui/
        └── button/
            ├── button.tsx          // Button, ButtonType
            ├── button.styles.ts    // buttonBase, withColor, buttonTypeStyles
            └── button.stories.tsx
```

`views/` отдельно от `components/` — оставил бы осознанно: это учебный MVVM-проект, разделение View/ViewModel/Model и есть цель. Но тогда `Views/View.tsx` обязан называться по имени компонента, иначе папка `views` содержит бессловесный `view.tsx`.

`src/main.tsx` (новый, точка входа):

```tsx
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import { ViewModel } from "./view-models/calculator-view-model";
import "./index.css";

const viewModel = new ViewModel();
const element = document.getElementById("root");

if (element != null) {
  createRoot(element).render(
    <StrictMode>
      <App viewModel={viewModel} />
    </StrictMode>,
  );
}
```

`src/App.tsx` (только компонент, без сайд-эффектов):

```tsx
import { CalculatorView } from "./views/calculator-view";
import type { ViewModel } from "./view-models/calculator-view-model";

interface AppProps {
  viewModel: ViewModel;
}

export default function App({ viewModel }: AppProps) {
  return <CalculatorView viewModel={viewModel} />;
}
```

`index.html` — две правки: `<script type="module" src="./src/main.tsx"></script>` и `<div id="root"></div>` (id в `getElementById` должен совпадать); заодно `<title>Document</title>` — это плейсхолдер Vite, тут был бы уместен `Practice Calculator`. Отступы в `index.html` — 4 пробела, во всех TS-файлах 2; без Prettier (пункт 8) это не сойдется само.

## 4. UI открывается надписью `Result: Error` — проверено прогоном

Прогон `ViewModel` в Node (установленные mobx 7.0.6 + mathjs 15.2.0):

```text
initial result: "Error"
initial expression: ""
after 2*3, result: "6"
after equals, string: "6"
after 62++ (invalid tail), result: "Error"
after clear, result: "Error"
backspace on empty: ""
```

Первая строка — это то, что пользователь видит при открытии страницы: пустое выражение → `evaluate("")` → `{ ok: false, reason: "empty" }` → геттер `result` (`Viewmodel.ts:13`) превращает **любой** `!ok` в `ERROR_TEXT`. Значит на старте и сразу после `CE` показывается «Error» там, где ошибки нет.

`reason: "empty"` для этого и заведён, но не читается (пункт 6). Правка в `Viewmodel.ts` — геттер целиком:

```ts
get result(): string {
  const evaluation = this.calculatedModel.evaluate(this.calculatedString);
  if (evaluation.ok) {
    return evaluation.display;
  }
  return evaluation.reason === "empty" ? "" : ERROR_TEXT;
}
```

Хорошая новость, зафиксированная прогоном: `2*3` действительно даёт живой `6`, бэксписейс на пустой строке не кидается (`"".slice(0, -1)` → `""`), `1/0` и `sqrt(-1)` не протекают в UI (ловятся `!Number.isFinite`). `equals()` и округление до 12 знаков работают.

## 5. Клавиши `=` нет — `equals()` недостижим, в сетке дырка

`grep` по `src` находит `equals` ровно в одном месте: определение в `Viewmodel.ts:31`. В массиве `keys` 19 элементов, `onPress: equals` нет ни у одного. То есть вся ветка «зафиксировать результат» мертва, и в сетке 4×5 пуста клетка (row 5, column 3) — соседи: `0` (5/1), `.` (5/2), `+` (5/4).

Предыдущая ревизия чинила `=`, который дописывался в строку как символ; сейчас его выпилили вместе с кнопкой. Если live-превью (`result`) и есть задуманное поведение — тогда удалите `equals()`, а не оставляйте недостижимый код. Если нет — добавьте клавишу:

```tsx
  {
    label: "=",
    buttonType: ButtonType.Action,
    column: 3,
    row: 5,
    onPress: equals,
  },
```

с хелпером рядом с остальными:

```tsx
const equals = (viewModel: ViewModel) => viewModel.equals();
```

Мелочь по уместности цвета: клавиша `.` размечена как `ButtonStyle.Operator` (пурпурная), хотя точка — часть числа, а не оператор.

## 6. `reason` нигде не читается; `className`-оверрайды выигрывают «по алфавиту»

`EvaluationResult.reason` — `union` из трёх значений (`empty | invalid | not-a-number`), но оба потребителя (`result`, `equals`) смотрят только в `ok`. Три разных состояния сведены в одно сообщение `Error` — либо покажите их разными текстами (тогда union оправдан), либо схлопните тип до `{ ok: false }`. Половинчатый вариант — мёртвый тип.

Про порядок классов в `className`. В `Button.tsx:44` склейка `[buttonTypeStyles[buttonType], className]` — «последний класс побеждает» здесь **не работает**, потому что в CSS побеждает не позиция в атрибуте, а позиция в файле. Разобрал `dist\assets\index-C6SRa9Lb.css`, порядок генерации:

```text
bg-blue-500@5582
bg-cyan-500@5634
bg-green-500@5686
bg-purple-500@5740
bg-red-500@5796
text-3xl@5937
text-lg@6032
text-black@6212
text-white@6249
```

Tailwind раскладывает утилиты одного свойства по алфавиту значения. Следствие: оверрайд `bg-*` из `className` выиграет только если он алфавитно **позже** цвета `buttonType` — например `bg-cyan-500` (5634) против красной цифры `bg-red-500` (5796) проиграет молча. `text-white` (6249) против `text-black` (6212) — выиграет, и история `CustomClassName` сейчас «случайно» зелёная именно поэтому. Это тот же класс тихой ошибки, что и с `col-start-${n}`: ни TS, ни консоль не ругаются.

Вариants: не смешивать в `className` утилиты того же свойства, что задаёт `buttonType`; либо `tailwind-merge` (`twMerge([...].filter(Boolean).join(" "))`) — но это новая зависимость, и для учебного проекта чище первый путь. Заодно `buttonUnset` (`Button.styles.ts:24`) удалён быть должен: grep по `src` использования не нашёл.

Стилистически в `Button.tsx`: `React.FC<ButtonProps>` используется без импорта `React` (работает за счёт UMD-глобала `@types/react`, `tsc` это принимает — проверено), при этом `CSSProperties` импортируется именованно. Либо `import type { CSSProperties } from "react"` и никакой `React.FC` (в React 19 типизация пропсов в сигнатуре функции — обычная практика), либо последовательный `import type { CSSProperties, FC } from "react"`. В `View.tsx:2` то же: `import { CSSProperties }` — это импорт **типа** как значения, просится `import type`.

## 7. mathjs: A/B измерено (закрываю прошлое «не проверено»)

Прошлый отчёт помечал экономию от `mathjs/number` как неизмеренную. Замерил настоящим `vite build` на копии проекта, меняя только строку импорта:

| вход              | модулей | JS            | JS gzip       |
| ----------------- | ------- | ------------- | ------------- |
| `"mathjs"`        | 1105    | 945,24 kB     | 278,21 kB     |
| `"mathjs/number"` | 718     | **654,22 kB** | **196,61 kB** |

−291,02 kB raw (−30,8 %) и −81,60 kB gzip (−29,3 %), при том что в app-bundle кроме mathjs остаётся весь React+ReactDOM+MobX. `tsc --noEmit` с `mathjs/number` тоже зелёный (exit 0).

Поведение на установленной 15.2.0, прогон в Node (левая колонка — `"mathjs"`, правая — `"mathjs/number"`):

```text
""          -> undefined            | undefined
"2+2"       -> 4                    | 4
"0.1+0.2"   -> 0.30000000000000004  | 0.30000000000000004
"2++3"      -> 5                    | 5
"1/0"       -> Infinity             | Infinity
"sqrt(-1)"  -> Complex, "i"         | NaN
"5!"        -> 120                  | 120
"3kg+2m"    -> throws Units...      | throws Undefined symbol kg
"x=5"       -> 5                    | 5
"2+"        -> throws SyntaxError   | throws SyntaxError
```

То есть для калькулятора переход строго лучше: `Complex` и `Unit` пропадают как классы значений (`NaN` и `Undefined symbol` ловятся текущими guard'ами, `reason` остаётся корректным), а `Infinity` отсекает `Number.isFinite`. Не меняется худшее: `2++3` → 5 и присваивание `x=5` → 5. Если хотите честный калькулятор, а не expression-движок, Whitelist входных символов (`/^[\d+\-*/(). ]+$/`) в `CalculationModel.evaluate` — одна проверка на границе.

Что осталось за кадром: CSS-файл в `dist-number` показался крупнее (`11,11 kB` против `8,14 kB`) — это артефакт замера (Tailwind просканировал предыдущий `dist-mathjs` внутри корня сборки), а не эффект `mathjs/number`; на ваш `src/index.css` переход не влияет. Точный вклад mathjs внутри единого чанка (manualChunks/анализатор) не мерил.

## 8. Мёртвые зависимости и инструменты, которых нет

Проверено по `package.json`, `.storybook/main.mjs` и списку файлов:

- **`@storybook/addon-vitest`, `vitest`, `@vitest/browser-playwright`, `@vitest/coverage-v8`, `playwright`** установлены и аддон зарегистрирован, но: тестовых файлов нет вообще (`dir /b /s src\*.test.* src\*.spec.*` → «File Not Found»), `test`-скрипта нет, `vitest.config.*` нет. `npm test` не определён → `npm test` падает. Либо 3–4 теста на `append/backspace/evaluate` (это как раз то, что уже проверено прогонами в пунктах 4 и 7), либо весь блок удалить — включая `playwright`, который тянет скачивание браузеров.
- **`@vitejs/plugin-react` отсутствует** (`node_modules\@vitejs\plugin-react` — нет; в `vite.config.mjs` только `tailwindcss()`). Следствие: в `npm run dev` нет Fast Refresh — каждое редактирование перезагружает страницу и **сбрасывает `calculatedString`**, что в учебном проекте мешает проверять состояние. JSX-рантайм при этом современный (`tsconfig.json → "jsx": "react-jsx"`), импорт `React` не нужен — это уже исправлено.
- `package.json: "\"type\": \"commonjs\""` для приложения, где весь код ESM (`.mjs` конфиги спасают, но поле врёт). Плюс нет `"private": true` — `npm publish` по ошибке выложит `20_practicecalculator` в реестр.
- `"main"` из прошлого отчёта убран ✓, `"description"`/`"keywords"`/`"author"` пустые (для private-проекта неважно).
- **Нет ESLint и Prettier** — ни одного конфига в проекте. Именно поэтому `React.FC` без импорта, `import type`, лишние импорты и смешанный регистр папок живут спокойно: `tsc` этих правил не проверяет, а `noUnusedLocals` ловит только приседания уровня «переменная не используется».
- `.storybook/main.mjs:4-7` — паттерн `"../src/**/*.mdx"` не находит ни одного файла (все `.mdx` удалены) → предупреждение на каждой сборке историй. Убрать строку или добавить `docs.mdx`.
- `tsconfig.json:21` — `"include": ["src", ".storybook"]` декоративно: `.storybook/*.mjs` без `allowJs` не проверяются. Полезно было бы добавить `verbatimModuleSyntax` (ловит «тип импортирован как значение» из абзаца выше) и `noUncheckedIndexedAccess`.

## 9. Что ещё в коде

- `disabled` и `className` в `ButtonProps` не использует ни один вызов в `View.tsx` (только истории). Для дизайн-системного компонента это нормально, но тогда стоит сказать об этом явно в названии пропа/типе, а не держать как «случайно не переданный».
- `@storybook/addon-a11y` зарегистрирован, но доступность клавиш не описана: `⌫` и `CE` без `aria-label` (скринридер читает `⌫` как «undefined»/ничего); `(` и `)` читаются нормально. `<output>` уже содержит текст `Result:` внутри → при каждом нажатии label перечитывается целиком; правильнее вынести подпись наружу (`<label>`/`aria-label`), а в `<output>` держать только значение.
- `<output>` имеет неявный `role="status"` (уже polite), явный `aria-live="polite"` — дубль, без вреда.
- `App.tsx` использует `<p>` для «Expression» и `<output>` для результата ✓ (прошлый пункт про два `<h1>` исправлен).
- Мелкое дублирование в `View.tsx`: `keys.map((key) => ...)` создаёт новый `onClick`-клажер на каждую клавишу каждый рендер — для 19 кнопок это не проблема, не «чинить».

## 10. Документация и история

- `js/react/suggestions.md` (прошлый отчёт «Ревизия проекта js/react — 2026-10-20») — **удалён из рабочего дерева и не закоммичен** (` D` в статусе). `styles_suggestions.md` — **0 байт**, изменение тоже незакоммичено. То есть вся предыдущая ревизия сейчас существует только в истории коммита `0dbe7f4`; при `git checkout .` она вернётся, при `git commit -a` — исчезнет. Прежде чем коммитить, решите, нужны ли эти файлы: «что чинили и почему» в учебном проекте — часть продукта.
- README нет ни в `20_practiceCalculator`, ни (прошлый раз) в упражнениях курса.
- `.qwen/` теперь в `.gitignore` (строка 1) — прошлое замечание закрыто ✓; `debug-storybook.log`, `dist`, `storybook-static` игнорируются ✓.
- Незакоммичено много разношёрстного: 20 удалённых файлов шаблонов `src/stories/*`, `M ../../../.gitignore`, `M index.html`, `M package.json`, `M tsconfig.json`. Разумно разбить на 2–3 коммита (шаблоны Storybook → до renaming/структура → отчёт), а не одним «Update the project».

## 11. Что исправилось с прошлой ревизии (проверено)

`=` больше не дописывается в строку (символа `=` в клавиатуре нет); вычисление уехало в `get result()` (computed, без `console.log` в рендере); guard по типу и `Number.isFinite` + `toPrecision(12)`; `tsconfig.json` появился (вместо «нет нигде»), `typescript` объявлен в devDependencies, `@types/mathjs` и `prop-types` удалены, `"main": "index.js"` убран; скрипты `build`/`preview`/`type-check` добавлены; цепочка `if` заменена на `Record<ButtonType, string>`; пустой `buttonTypes.ts` и `src/stories/*` удалены с диска; стили вынесены в `Button.styles.ts` с `col-start-(--col)`/`row-start-(--row)` (в CSS присутствуют — проверено на пунктом 6); `<button type="button">`; `aria-live`/`<output>`; клавиатура из массива через `.map()`; глобальный CSS импортируется в `.storybook/preview.mjs`.

## 12. Порядок правок

1. Закоммитить/начать отслеживать `Button.styles.ts` — без него проект не собирается даже на Windows (пункт 2).
2. Починить регистр в git-индексе (три команды из пункта 2), согласовав его с выбором из пункта 3. Идеально — сразу перейти на kebab-case, тогда расхождение исчезнет само.
3. `meta.args` + `buttonType` → `npm run type-check` зелёный (пункт 1).
4. `reason === "empty"` → не показывать «Error» на пустом вводе (пункт 4).
5. Решение по `=`: либо вернуть клавишу, либо удалить `equals()` (пункт 5).
6. `reason`: использовать по назначению или схлопнуть union; удалить `buttonUnset` (пункт 6).
7. `import { evaluate } from "mathjs/number"` → `npx vite build` и сравнить с 945,24 kB (пункт 7, ожидание ≈654 kB). Заодно whitelist символов от `x=5`/`2++3`.
8. Именование файлов/папок/пропсов + `main.tsx`/`App.tsx` + `index.html` (пункт 3).
9. Тесты или удаление vitest/playwright-блока; `@vitejs/plugin-react` для Fast Refresh (пункт 8).
10. ESLint + Prettier (иначе пункты 3 и 6 вернутся через неделю).
11. `.mdx`-паттерн в `.storybook/main.mjs`, `"private": true`, `"type": "module"`, README, решить судьбу старых отчётов (пункты 8, 10).

## 13. Что не проверялось

- Поведение в браузере (Browser Use на этой машине не работает): скриншотов/кликов не было, выводы по UI сделаны из прогонов `ViewModel`/`mathjs` в Node и из чтения кода.
- Что сборка **из чистого клона** падает — не выполнялось (не делал `git clone` и не трогал случайную ФС); это вывод из состояния git-индекса (`git ls-files`, `ignorecase=false status`, `--error-unmatch`) и семантики регистрозависимых ФС.
- Точный вклад mathjs внутри единого чанка и эффект `manualChunks` — не измерялось.
- Доступность (озвучка `⌫`, перечитывание `<output>`) — не проверялась скринридером; a11y-аддон я не запускал.
- `npm run build-storybook` зелёный, но сами истории в браузере не открывал; после правки `meta.args` истории не пересматривались на предмет «что изменилось в сайдбаре».
- Совместимость `mathjs/number` с `strict`-проверками типовBeyond `tsc --noEmit` (например `vitest`-тайпинги) — не проверялась.
