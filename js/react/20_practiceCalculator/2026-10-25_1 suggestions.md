# Ревизия проекта `20_practiceCalculator` — 2026-10-25, проход `_1`

Третий проход. Ветка `js2`, `HEAD = 86b6e6e "Changes"`, рабочее дерево **чистое** — все правки после прошлой ревизии уже закоммичены, поэтому ниже разобран не `git diff`, а актуальное состояние исходников + сравнение с пунктами отчёта `2026-10-25 suggestions.md`.

Главное: из пяти регистровых расхождений осталось одно, но именно оно **ломает сборку чистого клона** — и на этот раз доказано реальным падением `vite build`, а не выводом из состояния индекса. Второе по重要ости — новое: live-preview показывает `Error` в процессе нормального набора (`2+`, `2+3*`), потому что незавершённое выражение неотличимо от неверного.

## Как проверялось

Реально прогнанные команды (все — из папки проекта):

- `npm run type-check` → **exit 0** (в прошлом прогоне падал с `TS2322`);
- `npx vite build` → exit 0, **1106 модулей**, `dist/assets/index-l2pAEHES.js` **945,14 kB** (gzip 277,98 kB), `dist/assets/index-CehiccHw.css` 8,17 kB (gzip 2,45 kB);
- `npm run build-storybook` → exit 0, предупреждение `No story files found for the specified pattern: src\**\*.mdx` **по-прежнему есть**;
- `git status` → `nothing to commit, working tree clean`; `git ls-files` → в индексе `src/app.tsx`; `dir /b /s src` → на диске `src\App.tsx`;
- `git -c core.ignorecase=false status --porcelain` → ` D src/app.tsx` + `?? src/App.tsx` (единственное оставшееся расхождение; остальные четыре из прошлого отчёта закрыты);
- **тест регистрозависимой ФС**: `fsutil file setCaseSensitiveInfo .qwen\tmp\cs enable` (атрибут подтверждён `queryCaseSensitiveInfo`), в эту папку развёрнуты файлы **по именам из git-индекса** (`git ls-files` → запись байт-в-байт), затем `npx vite build` → **ПАДЕНИЕ**:
  ```text
  ✓ 3 modules transformed.
  ✗ Build failed in 78ms
  error during build:
  Could not resolve "./App" from "src/main.tsx"
  ```
  оба лечения прогнаны там же: правка спекса на `"./app"` → **BUILD OK**; переименование файла в `App.tsx` → **BUILD OK**;
- `.qwen\tmp\casecheck.cjs` — разбор всех относительных `import` по индексу с учётом регистра: единственный `FAIL` — `src/main.tsx -> ./App`; все остальные (9 спексов) разрешаются;
- поведение `ViewModel` прогнано в Node: `npx esbuild .qwen\tmp\cs\vmprobe.ts --bundle --platform=node --format=cjs` + `node` (таблица в пункте 2);
- лечение из пункта 2 проверено так же на копии: `.qwen\tmp\cs\fixcrun.ts` → `node` (таблица в пункте 2);
- `mathjs` прогнан на установленной 15.2.0: `.qwen\tmp\mathprobe.cjs` (20 строк) + полный `CalculationModel.evaluate` (28 входов, перебор всех 1–2-символьных строк из алфавита клавиатуры) — пункт 7;
- MobX прогнан: `isObservable(vm)`, `isObservableProp(vm, "expression"|"evaluator"|"result")`, `isObservable(vm.evaluator)` → **false**; кэширование computed: 3 чтения без observer → **3** вызова `evaluate`, с `autorun` → **1** вызов на 3 чтения, после мутации → **2** (пункт 9);
- `npx vitest run src/models/CalculationModel.test.ts --root .` на копии проекта → тест-раннер **работает без единого конфига**: 4 теста, 2 прошли, 2 упали (мои ожидания, не код — см. пункт 8);
- `node .qwen\tmp\cs\tailwindorder.cjs` по актуальному `dist\assets\index-CehiccHw.css` — порядок утилит (пункт 5);
- наличие инструментов проверено `fs.existsSync` по девяти именам конфигов + glob по `src` на `*.test.*|*.spec.*|*.mdx` → **ни одного файла**;
- `git cat-file -s HEAD:...styles_suggestions.md` → `0`; `git cat-file -s 0dbe7f4:...styles_suggestions.md` → **27943** (пункт 11);
- чтение всех непустых исходников и конфигов.

Рабочие файлы проекта **не изменены** (`git status` после всех прогонов — `working tree clean`). Временное: `.qwen\tmp\casecheck.cjs`, `.qwen\tmp\mathprobe.cjs`, `.qwen\tmp\build.log`, `.qwen\tmp\sb.log`, `.qwen\tmp\cs\` (копии, прогины, `dist-cs`) — удалены после проверок, атрибут case-sensitivity с `.qwen\tmp\cs` снят (`fsutil ... disable`). Честное предупреждение как в прошлый раз: прогоны `vite build` и `build-storybook` перезаписали `dist\` и `storybook-static\` — gitignore-нутые артефакты, пересоздаются вашими же скриптами.

---

## 1. Критично: чистый клон не собирается — `main.tsx` импортирует `"./App"`, а в индексе лежит `src/app.tsx`

Это уже не «подозрение по состоянию индекса», а воспроизведённое падение. В прошлый раз индекс и диск расходились в пяти местах; правки из пункта 3 прошлого отчёта приняты (папки стали `models/`, `view-models/`, `views/`, `components/ui/button/`), но по самому файлу точки монтажа расхождение осталось, и теперь оно **внутри компонента, который всё связывает**:

| git-индекс (`git ls-files`) | диск (`dir /b /s src`) | что пишет код |
| --------------------------- | ---------------------- | ------------ |
| `src/app.tsx`              | `src\App.tsx`          | `src/main.tsx:3` → `import App from "./App"` |

Локально всё зелёное по одной причине: `git config core.ignorecase` = `true` и NTFS без атрибута case-sensitivity не различает регистр. Проверено, что `tsc` этого **не видит в принципе**: обе записи разрешаются в один и тот же файл на диске, поэтому `forceConsistentCasingInFileNames` тут не помощник — поймать можно только на регистрозависимом стенде (что я и сделал выше).

В клоне (Linux-CI, Docker, WSL2 с native-ФС, macOS на кейс-сенситивном томе) файл называется `app.tsx`, спекс `"./App"` не резолвится → сборка падает на первых трёх модулях с `Could not resolve "./App" from "src/main.tsx"`. То есть **деплой/CI из этого коммита красный**, а `npm run build` на вашей машине — зелёный, и это ровно тот класс ошибки, который не лечится перезапуском.

Лечение B (рекомендую): сделать `App.tsx` истиной и в индексе, и на диске — тогда и импорт менять не надо, правка чисто git-овая. Диск уже `App.tsx`, значит приводим индекс:

```cmd
cd /d C:\other\SelfEducation\js\react\20_practiceCalculator
git -c core.ignorecase=false rm -r --cached src
git add src
git status --short
```

`git rm --cached` рабочий дерево не трогает, только индекс; после этого в индексе будет ровно тот регистр, что на диске, а `git -c core.ignorecase=false status --porcelain` должен стать **пустым**. Проверенный ориентир: в scratch-копии с `src/App.tsx` в регистрозависимой папке `vite build` → BUILD OK.

Лечение A (минимальное, одна строка, но расхождение индекса и диска остаётся): `import App from "./app"` — тоже проверено, BUILD OK. Минус: индекс продолжит хранить `app.tsx`, а диск — `App.tsx`, и при следующем `git add` регистр может снова «уплыть». Если выбираете A, переименуйте и на диске (на Windows в один шаг `App.tsx` → `app.tsx` не переименовывается, нужен промежуточный `git mv src/App.tsx src/App.tmp.tsx && git mv src/App.tmp.tsx src/app.tsx`).

Вариант A оставляет дерево в стиле «все файловые имена строчные» (`main.tsx`, `index.css`, `app.tsx`), вариант B — в стиле «компоненты с PascalCase». Оба законны; **незаконно только расхождение индекса и диска**, потому что оно не видно на машине, где сделано.

Отдельно: добавьте в CI (или в pre-commit) тот же тест, что я прогнал, — он стоит three строк и ловит этот класс ошибок до коммита:

```cmd
git -c core.ignorecase=false status --porcelain || exit /b 1
```

если вывод непустой — индекс и диск разошлись по регистру.

## 2. `Error` при нормальном наборе: `2+` и `2+3*` дают `Error` в live-preview

Проверено прогоном актуального `ViewModel` в Node (установленные mobx 7.0.6 + mathjs 15.2.0):

```text
initial                   expr=""          result=""
press 2                   expr="2"         result="2"
press + (mid-typing)      expr="2+"        result="Error"
press 3 -> 2+3            expr="2+3"       result="5"
12++ (double operator)    expr="12++"      result="Error"
backspace -> 12+          expr="12+"       result="Error"
open paren tail           expr="12*3("     result="Error"
close paren               expr="12*3()"    result="Error"
backspace x3 on empty     expr=""          result=""
```

Хорошая новость: прошлый Critical закрыт — на старте и после `CE` больше нет `Error` (`reason === "empty"` теперь действительно читается), бэксписейс на пустой строке не кидается. Плохая новость: с тех пор как убрали `=`, **каждое** нажатие оператора показателю строку `Result: Error`, потому что в getter'е любой `!ok`, кроме `empty`, — это `ERROR_TEXT`. То есть `Error` теперь штатное состояние на полэкрана при наборе, а не сигнал ошибки. `mathjs` на `2+` бросает `SyntaxError` → ветка `invalid` → `Error`; незавершённое выражение и неверное схлопнуты в одно сообщение.

Лечение — четвёртый `reason` для синтаксически незавершённого ввода, но ещё не ошибочного. Прогнано на копии: `2+` → `""`, `2+3*` → `""`, `2++` → `""`, `2+(` → `""`, при этом `2+()` → `Error`, `1/0` → `Error`, `0/0` → `Error`.

`src/models/CalculationModel.ts` — файл целиком:

```ts
import { evaluate as mathEvaluate } from "mathjs";

const INCOMPLETE_TAIL = /[+\-*/.(]\s*$/;

export type EvaluationResult =
  | { ok: true; display: string }
  | { ok: false; reason: "empty" | "incomplete" | "invalid" | "not-a-number" };

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
      return INCOMPLETE_TAIL.test(input)
        ? { ok: false, reason: "incomplete" }
        : { ok: false, reason: "invalid" };
    }
  }
}
```

`src/view-models/calculator-view-model.ts` — файл целиком (единственные отличия от текущего: аннотация `evaluator: false` и ветка `incomplete`; см. пункт 9, почему `false`):

```ts
import { makeAutoObservable } from "mobx";
import { CalculationModel } from "../models/CalculationModel";

const ERROR_TEXT = "Error";

export class ViewModel {
  evaluator: CalculationModel = new CalculationModel();
  expression: string = "";

  constructor() {
    makeAutoObservable(this, { evaluator: false });
  }

  get result(): string {
    const evaluation = this.evaluator.evaluate(this.expression);
    if (evaluation.ok) {
      return evaluation.display;
    }
    if (evaluation.reason === "empty" || evaluation.reason === "incomplete") {
      return "";
    }
    return ERROR_TEXT;
  }

  appendChar = (input: string): void => {
    this.expression += input;
  };

  pressBackspace = (): void => {
    this.expression = this.expression.slice(0, -1);
  };

  pressClear = (): void => {
    this.expression = "";
  };
}
```

Замечание по честности: проверка логики делалась в копии, где `mathjs` подключался через `require` (прогон в Node), а в проекте — через `import { evaluate } from "mathjs"`; строка импорта в вашем файле не меняется, меняется только поток `reason`. И ещё: `union` из четырёх значений теперь оправдан — все четыре читаются.

## 3. `interface AddProps` — опечатка в имени, появилась в этом коммите

`src/app.tsx:4`: пропсы `App` называются `AddProps`. `tsc` это устраивает (имя валидное), поэтому проверка типов не поймает, а читатель — споткнётся: `Add` рядом с `appendChar` из ViewModel читается как «добавить пропсы».

`src/app.tsx` — файл целиком:

```tsx
import { ViewModel } from "./view-models/calculator-view-model";
import { CalculatorView } from "./views/calculator-view";

interface AppProps {
  viewModel: ViewModel;
}

export default function App({ viewModel }: AppProps) {
  return <CalculatorView viewModel={viewModel} />;
}
```

## 4. Именование: три корня для одной сущности никуда не делись, плюс устаревший комментарий в историях

Пункт 3 прошлого отчёта принят частично. Что осталось и чем это стоит:

| Сейчас | Что не так | Как принято |
| ------ | ---------- | ----------- |
| `view-models/calculator-view-model.ts` → `class ViewModel` | имя файла говорит `calculator`, имя класса — нет; в MVVM `ViewModel` без префикса нельзя найти по поиску и нельзя положить второй экран | `class CalculatorViewModel` (файл уже называется правильно) |
| `views/calculator-view.tsx` → компонент `CalculatorView`, пропсы `CalculationViewProps` | `Calculation` vs `Calculator` в соседних строках одного файла; корень один на всё: `Calculator` | `CalculatorViewProps` |
| хелперы в клавиатуре: `append` / `clear` / `removeLast`, метод VM: `appendChar` / `pressClear` / `pressBackspace` | два имени для одной операции; `removeLast` vs `pressBackspace` — «backspace» исчез из словаря, хотя клавиша `⌫` именно она | либо `pressAppend`/`pressClear`/`pressBackspace`, либо `append`/`clear`/`backspace` — но один набор везде |
| `Button.stories.tsx:71` комментарий «`Number` is a JS global, so the local names avoid shadowing it» + `export { NumberKey as Number, ManageKey as Manage }` | комментарий стал **ложным**: члена `ButtonType.Number` больше нет (есть `Digit`), затенять нечего; при этом в сайдбаре истории называются `Number` и `Manage`, а enum — `Digit` и `Action` | снять алиасы и объявить `export const Digit: Story` / `Action: Story` прямо; комментарий удалить |
| `title: "UI/CalculatorButton"` | не совпадает ни с именем компонента (`Button`), ни с путём (`components/ui/button`) | `"Components/Button"` — или, если нужен раздел, `"UI/Button"` |

Мелочь по сути, а не по стилю: `CalculatorViewProps`/`ViewModel` расходятся с именами файлов, и при переходе на автоимпорт по имени файла (Vite alias, `@/view-models/CalculatorViewModel`) это придётся распутывать руками.

## 5. Перенос из прошлого отчёта, не исправлено: оверрайд `bg-*` из `className` молча проигрывает

`Button.tsx:44` склейка не менялась. Перемерил порядок на **актуальном** `dist\assets\index-CehiccHw.css`:

```text
bg-blue-500@5582   bg-cyan-500@5634   bg-green-500@5686   bg-purple-500@5740   bg-red-500@5796
text-black@6212    text-white@6249
победитель: bg-cyan-500 vs bg-red-500 -> WINS bg-red-500
победитель: text-white vs text-black  -> WINS text-white
```

Механика та же: в одном свойстве Tailwind раскладывает утилиты по алфавиту значения, а не по порядку в атрибуте `class`. Следствие для дизайн-системного компонента: `className="bg-cyan-500"` для цифры (`bg-red-500`@5796) **не сработает** — ни ошибки, ни предупреждения; `text-white` сработает только потому, что `white`alphabetically позже `black`. История `CustomClassName` до сих пор выглядит правильно «по случайности».

Лечение без новых зависимостей (предпочтительнее для учебного проекта): не смешивать в `className` утилиты того же свойства — `text-*`/`bg-*` не перекрывать, если цвет задал `buttonType`; либо перенести цвет из `buttonBase`-наследия в явный аргумент и убрать `text-black` из базы. С зависимостью — `tailwind-merge`: `twMerge([buttonTypeStyles[buttonType], className].filter(Boolean).join(" "))`.

## 6. Сетка 4×5 с пустой клеткой и точка, помеченная как оператор

`equals()` из ViewModel удалён, клавиши `=` в `keys` нет — прошлый пункт 5 закрыт выбором «live-preview вместо фиксированного результата», это валидное решение и мёртвого кода больше нет. Но остались два следствия:

- в `keys` 19 элементов, сетка `repeat(4, 1fr) / repeat(5, 1fr)` = 20 ячеек, клетка `(row 5, column 3)` пуста: соседи `0` (5/1), `.` (5/2), `+` (5/4). Дырка видна на экране;
- `label: "."` размечен как `ButtonType.Operator` → пурпурная, хотя точка — часть числа; рядом `ButtonStyle.Digit` уже есть.

Если live-preview и есть задуманное поведение, то закрывается это одним решением: убрать строку `.`/переставить `+` на 3-ю колонку (или растянуть `0` на две колонки через `colSpan`) и перекрасить `.` в `Digit`. Если результат всё же нужно «фиксировать» (например, `2+3` → `5`, затем `+` продолжает от `5`), то `equals()` должен вернуться вместе с клавишей — иначе калькулятор не умеет базовую вещь, а отсутствие `=` — это не фича, а дырка в сетке.

## 7. `mathjs`: что реально достижимо с этой клавиатуры и зачем whitelist

Клавиатура производит только алфавит `0123456789+-*/().` и два слова `CE`/`⌫` (не попадают в строку). Перебрал все 1–2-символьные строки этого алфавита через ваш `CalculationModel` и отдельно 28 содержательных входов; вот то, что **достижимы именно этой клавиатурой** и даёт странный, но «успешный» ответ:

```text
"2++3"    -> { ok: true, display: "5" }     // унарный плюс: 2 + (+3)
"7--2"    -> { ok: true, display: "9" }     // 7 - (-2)
"2(3)"    -> { ok: true, display: "6" }     // неявное умножение
"+2"      -> { ok: true, display: "2" }
"1/0"     -> { ok: false, reason: "not-a-number" }   // Infinity, отсекается верно
"0/0"     -> { ok: false, reason: "not-a-number" }   // NaN, отсекается верно
"999999999999999999999" -> { ok: true, display: "1e+21" }
```

А это **недостижимо** с этой клавиатуры и не является дырой (в отличие от прошлого отчёта, где я считал `x=5` реальной проблемой — для него нужны буквы, а букв на клавиатуре нет): `x=5` → `5`, `3kg` → `Unit`, `sqrt(-1)` → `Complex`, `50%` → `0.5`. То есть guard'ы `typeof !== "number"` и `Number.isFinite` уже закрывают всё достижимое: `Infinity`/`NaN` → `Error`, и в UI никогда не протекают `Complex`/`Unit`.

Whitelist всё равно нужен, но по другой причине: `2++3` → `5` и `2(3)` → `6` — это не «безопасно», это другой калькулятор. Одна проверка на границе, перед `mathEvaluate`:

```ts
const ALLOWED_INPUT = /^[\d+\-*/(). ]+$/;
```

и `return { ok: false, reason: "invalid" }` при несовпадении — плюс, если хотите честную арифметику, отказ при `/[+\-*/]{2,}/` внутри строки после первого оператора.

Эффект перехода на `mathjs/number` (прошлое измерение: 945,24 kB → 654,22 kB, 1105 → 718 модулей) в этом прогоне **не перемерялся**; актуальный `vite build` даёт 945,14 kB / 1106 модулей, то есть с прошлой ревизии вес не изменился и пункт остаётся открытым. Отмечу разницу, которую thấy в прогонах и не разобрал: в Node-бандле `sqrt(-1)` даёт `not-a-number` (Complex → не `number`), а в прогоне через `vitest`/vite-node тот же вход дал `invalid` (mathjs бросил). На UI не влияет — оба пути рисуют `Error` — но тесты на конкретное значение `reason` для не-чисел писать нельзя, пока не разобрались с тем, как mathjs линкуется в тестовом стенде.

## 8. Перенос: инструменты куплены и не используются, но тесты стоят дешевле, чем казалось

Проверено `fs.existsSync` по конфигам и glob'ом по `src`:

- тестовых файлов **ноль** (`*.test.*`, `*.spec.*`, `*.mdx` в `src` — ни одного), `test`-скрипта в `package.json` нет, `vitest.config.*`/`vitest.workspace.ts` нет → `npm test` не определён. При этом `@storybook/addon-vitest` зарегистрирован в `.storybook/main.mjs`, а `vitest`, `@vitest/browser-playwright`, `@vitest/coverage-v8`, `playwright` — в зависимостях.
- **Хорошая новость, которую я проверил прогоном**: `npx vitest run src/models/CalculationModel.test.ts --root .` на копии проекта запустился и отработал (4 теста, 2 passed / 2 failed — упали из-за моих же ошибок в ожиданиях: `0.1+0.2` реально даёт `"0.3"`, а не `"0.1"`). То есть конфиг не нужен, достаточно одной строки в `package.json`: `"test": "vitest run"`, и 3–4 теста на то, что уже доказано прогонами в пунктах 2 и 7 (`empty`/`incomplete`/`invalid`/`not-a-number`, `toPrecision(12)`). `playwright`-блоки в таком случае можно оставить; без теста на них — это скачанные браузеры ради мёртвой зависимости.
- `@vitejs/plugin-react` **по-прежнему нет** (`vite.config.mjs`: только `tailwindcss()`), `.qwen`-проверка `node_modules\@vitejs\plugin-react` — нет. В `npm run dev` нет Fast Refresh: правка `Button.tsx` перезагружает страницу и **сбрасывает `expression`**, то ровно то состояние, которое в учебном проекте вы и учитесь наблюдать.
- ESLint и Prettier — ни одного конфига (проверено девять имён). Именно поэтому живут: `AddProps` (пункт 3), `import { CSSProperties } from "react"` как импорт **значения** вместо `import type` — в `Button.tsx:1` и `views/calculator-view.tsx:2`, и `React.FC<ButtonProps>` при неимпортированном `React` (`Button.tsx:33`, работает за счёт UMD-глобала из `@types/react`, `tsc` принимает — проверено exit 0).
- `.storybook/main.mjs` — паттерн `"../src/**/*.mdx"` не находит файлов, `build-storybook` печатает предупреждение (проверено в этом прогоне). Убрать строку или добавить `docs.mdx`.
- `package.json`: `"type": "commonjs"` при полностью ESM-коде, `"private": true` нет → `npm publish` по ошибке выложит `20_practicecalculator` в реестр.
- `index.html`: `<title>Document</title>` — плейсхолдер Vite до сих пор не тронут; отступы 4 пробела против 2 во всех TS-файлах.
- `README.md` нет.

## 9. MobX: computed кэшируется только под observer, и вложенный Model не наблюдаем — это надо знать, а не чинить

Прогон на установленной mobx 7.0.6:

```text
isObservable(vm)                       = true
isObservableProp(vm, "expression")      = true
isObservableProp(vm, "result")          = true      // computed
isObservableProp(vm, "evaluator")       = true
isObservable(vm.evaluator)              = false     // вложенный Model НЕ обернут
3 чтения vm.result без observer         = 3 вызова evaluate()
3 чтения vm.result с autorun            = 1 вызов; + мутация и чтение = 2 вызова
```

Выводы по порядку:

1. `result` — настоящий computed, и в приложении (`CalculatorView` обёрнут в `observer`) кэш работает: одно пересчёт на изменение `expression`. Но **вне** реактивного контекста (консоль, тест, `console.log(vm.result)` в дебаге) каждое чтение пересчитывает `mathjs` заново. Это не баг, а то, что нужно учитывать при отладке: «тормозит в консоли» ≠ «тормозит в UI».
2. `isObservable(vm.evaluator) === false` подтверждает то, что уже находили раньше: `makeAutoObservable` не залезает во вложенные классы Model. Для `CalculationModel` это безразлично — у него нет состояния, только метод. Тогда правильнее сказать MobX'у это явно: `makeAutoObservable(this, { evaluator: false })` (как в пункте 2) — сервис перестаёт быть observable-полем, а `evaluator: false` читается как документация: «тут состояние не живёт». Если в `CalculationModel` появится состояние (история вычислений, память, режим угла), этот флаг надо снять и вызвать `makeAutoObservable` внутри Model — иначе состояние будет меняться молча и UI не обновится.
3. Методы-стрелки (`appendChar`, `pressBackspace`, `pressClear`) — собственные поля, MobX делает из них actions, поэтому `enforceActions: "observed"` (дефолт v7) не ругается. Ругань `Since strict-mode is enabled, changing (observed) observable values without using an action` в моих прогонах появилась только потому, что я сам присваивал `vm.expression = "6"` в обход методов, — в вашем коде такого пути нет. Не «чинить».

## 10. Перенос: доступность клавиш и `Result:` внутри live-зоны

Не проверено скринридером (на этой машине Browser Use не работает, a11y-аддон я не запускал) — это стандартные паттерны, а не измерение:

- `⌫` и `CE` не имеют `aria-label`: озвучка `⌫` зависит от голоса, `CE` читается как буква-буква. В `ButtonProps` нет пропа под `aria-label`, хотя `label` — визуальный текст;
- текст подписи лежит **внутри** live-зоны, поэтому при каждом наборе перечитывается и `Result:` (а до правки из пункта 2 — и `Error` целиком). Правильный минимум — подпись вынести за `aria-live`, внутри держать только значение:

```tsx
      <p className="text-3xl font-bold">Expression: {viewModel.expression}</p>
      <p className="text-3xl font-bold">
        Result: <output aria-live="polite">{viewModel.result}</output>
      </p>
```

- `disabled` и `className` в `ButtonProps` не использует ни один вызов из `calculator-view.tsx` (только истории) — для компонентной библиотеки это нормально, но тогда стоит сказать это в названии/типе, а не держать как «случайно не переданное».

## 11. Документация: `styles_suggestions.md` закоммичен пустым, но содержимое живо в истории

Проверено по объектам git, а не по рабочему дереву:

- `git cat-file -s HEAD:js/react/20_practiceCalculator/styles_suggestions.md` → **0**, а в `0dbe7f4` тот же путь весил **27 943** байта. Прошлый отчёт предупреждал «обрезан до 0 байт, изменение незакоммичено» — оно ушло в коммит, так что обнуление теперь зафиксированное состояние, а не временное. Восстановление:
  ```cmd
  git show 0dbe7f4:js/react/20_practiceCalculator/styles_suggestions.md > styles_suggestions.md
  ```
- `js/react/suggestions.md` (ревизия всего `js/react` от 2026-10-20) удалён с диска и тоже лежит в истории (`git log` по этому пути даёт `0dfe3cc`, `242c636`); тот же `git show 242c636:...` его вернёт;
- отчёт `2026-10-25 suggestions.md` (40 384 байта) в индексе есть ✓ — прошлое замечание «вся предыдущая ревизия существует только в истории» закрыто. Пробел в имени файла оставлен осознанно (вы второй раз задаёте имя с пробелом: `2026-10-25_1 suggestions.md`) — пункт из прошлого отчёта снимаю, это ваш house style, а не ошибка;
- коммит `86b6e6e` называется `Changes` при девяти файлах разношёрстного (переименования + новый VM + новая точка входа + истории). Прошлая рекомендация разбить на 2–3 коммита осталась; сейчас это тем более стоит сделать, потому что регистровую правку из пункта 1 удобно выделять в отдельный коммит (`git -c core.ignorecase=false rm -r --cached src` + `git add src` в одном диффе с прочим читается как «переименовано всё», хотя переименовано одно).

## 12. Что исправилось с прошлой ревизии (проверено)

- `npm run type-check` → exit 0: `meta.args` получил обязательный `buttonType`, `AllTypes` больше не требует `args` (прошлый пункт 1);
- `Button.styles.ts` в индексе ✓ (прошлый пункт 2, «не собирается даже на Windows»);
- регистр папок: `src/models`, `src/view-models`, `src/views`, `src/components/ui/button` — четыре из пяти расхождений закрыты, `Viewmodels` → `view-models` (и орфография, и стиль), `views/view.tsx` → `views/calculator-view.tsx` (файл = имя компонента), `buttons` → `button` (единственное число);
- `reason === "empty"` читается: на старте и после `CE` больше нет `Result: Error` (прошлый пункт 4);
- мёртвый код: `equals()` без клавиши удалён; `buttonUnset` удалён из `Button.styles.ts`;
- переименования из таблицы прошлого пункта 3 приняты: `calculatedString` → `expression`, `calculatedModel` → `evaluator`, `addChar` → `appendChar`, `interface Key` → `KeypadButton`, `ButtonType.Number` → `Digit`, `ButtonType.Manage` → `Action`, `keypadStyle`, `buttonTypeStyles`;
- `main.tsx` выделен в точку входа с побочными эффектами, `App` стал чистым компонентом с пропсами; `index.html` → `./src/main.tsx`, `<div id="root">` ✓, `<App viewModel={viewModel} />` вместо `createRoot` внутри `App.tsx`;
- `.qwen/`, `dist`, `storybook-static`, `debug-storybook.log` игнорируются; рабочее дерево чистое, отчёт в гите.

## 13. Порядок правок

1. Привести индекс к `App.tsx` (лечение B из пункта 1) — до всего остального, потому что CI/клон красный, а локально это не видно. Проверка: `git -c core.ignorecase=false status --porcelain` пустой **и** `npm run build` зелёный.
2. `reason: "incomplete"` + `makeAutoObservable(this, { evaluator: false })` (пункт 2, полные файлы) — убрать `Error` при наборе. Проверка: тот же прогон-последовательность, `2+` → `""`, `2+()` → `Error`.
3. `AddProps` → `AppProps` (пункт 3).
4. Решить сетку: `.` → `Digit`, закрыть клетку (5,3) или вернуть `equals()` с клавишей `=` (пункт 6).
5. `import type { CSSProperties } from "react"` в `Button.tsx` и `calculator-view.tsx`; `React.FC` → типизация пропсов в сигнатуре; `title: "Components/Button"`; снять алиасы `NumberKey as Number` / `ManageKey as Manage` и ложный комментарий про затенение (пункты 4, 5, 8).
6. Класс `ViewModel` → `CalculatorViewModel`, `CalculationViewProps` → `CalculatorViewProps`, один набор глаголов для хелперов и методов (пункт 4).
7. `"test": "vitest run"` + 3–4 теста на `evaluate` — раннер уже работает, проверено (пункт 8). Заодно `@vitejs/plugin-react` в `vite.config.mjs` для Fast Refresh.
8. Whitelist ввода `/^[\d+\-*/(). ]+$/` и отказ на `2++3`/`2(3)` (пункт 7).
9. `import { evaluate } from "mathjs/number"` → `npx vite build` и сравнить с 945,14 kB (ожидание ≈654 kB, цифра прошлого прогона) (пункт 7).
10. ESLint + Prettier — иначе пункты 3–5 вернутся через неделю.
11. Мелкое: `<title>Practice Calculator</title>`, `"private": true`, `"type": "module"`, убрать `../src/**/*.mdx` из `.storybook/main.mjs`, README.
12. Вернуть `styles_suggestions.md` из `0dbe7f4`, если он ещё нужен; коммитить регистровую правку отдельно от содержательных (пункт 11).
13. a11y: `aria-label` для `⌫`/`CE`, подпись `Result:` вне live-зоны (пункт 10).

## 14. Что не проверялось

- **Настоящий `git clone` на регистрозависимой ФС не делал.** Падение получено в папке с включённым `fsutil ... setCaseSensitiveInfo`, куда файлы развёрнуты по именам из `git ls-files` — механика разрешения путей та же, что у Linux/CI, но это не тот же самый стенд (не проверял поведение git-клиента при клонировании, не проверял CI-раннер).
- Поведение в браузере: ни скриншотов, ни кликов. Все выводы по UI — из прогонов `ViewModel`/`mathjs`/`mobx` в Node и чтения кода. После пункта 2 в браузере стоит посмотреть реально: мигает ли пустая строка результата при быстром наборе.
- Доступность: скринридером не проверял, `@storybook/addon-a11y` не запускал.
- `npm run build-storybook` зелёный, но истории в браузере не открывал; после правок `title`/алиасов из пункта 4 не смотрел, что изменилось в сайдбаре.
- Эффект `mathjs/number` на актуальное дерево **не перемерял** (в этом прогоне только базовые 945,14 kB / 1106 модулей); точный вклад mathjs внутри единого чанка и `manualChunks` — как и в прошлом разе, не измерялось.
- Разница `reason` для `sqrt(-1)` между Node-бандлом (`not-a-number`) и vite-node-прогоном (`invalid`) — зафиксирована, не разобрана.
- Регулярка `INCOMPLETE_TAIL` проверена на 28 входах прогона, но не на всём пространстве строк клавиатуры: в частности, выражения вида `2+*3` и `((2)` попадают в `incomplete`/`invalid` по-разному и это стоит покрыть тестами из пункта 13.7, а не моей выборкой.
