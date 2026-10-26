# Ревизия проекта `20_practiceCalculator` — 2026-10-26

Четвёртый проход. Ветка `js2`, `HEAD = de7ec8c "Some changes"`, рабочее дерево **чистое**. Разобрано: актуальное состояние исходников + сверка с пунктами отчёта `2026-10-25 suggestions.md` и `2026-10-25_1 suggestions.md`.

Главное. Правая часть прошлого Critical закрыта (`src/app.tsx` → `src/App.tsx` в индексе, `npm run type-check` зелёный), но **тот же класс ошибки вернулся в другом месте и теперь доказан падающей сборкой**: в `HEAD` папка лежит как `src/components/ui/button/`, а `src/views/calculator-view.tsx` импортирует `"../Components/Ui/Button/Button"`. На регистрозависимой ФС чистый клон **не собирается** — `Could not resolve "../Components/Ui/Button/Button"`, падение после 21 модуля из 1106.

Второе по важности — новое: `Error` в live-preview при наборе **незакрытой скобки** (`(2+3`, `((1+2)`). Лечение из прошлого отчёта (`reason: "incomplete"`) считает ввод незавершённым только в одном случае — когда строка *заканчивается* на оператор; выражение, где пользователь уже набрал `(2+3` и собирается закрыть скобку, попадает в ветку `invalid`.

## Как проверялось

Реально прогнанные команды и проверки (все — из папки проекта, если не указано иное):

- `git status` → `nothing to commit, working tree clean`; `git log --oneline -6` → `de7ec8c`, `2d7c450`, `86b6e6e`, …
- `npm run type-check` → **exit 0** (в прогоне 2026-10-25 падал с `TS2322`);
- `npx vite build` → exit 0, **1106 модулей**, `dist/assets/index-GmUccAtO.js` **945,25 kB** (gzip 278,03 kB), `dist/assets/index-CehiccHw.css` 8,17 kB (gzip 2,45 kB), предупреждение `Some chunks are larger than 500 kB`;
- `npm run build-storybook` → exit 0, предупреждение `No story files found for the specified pattern: src\**\*.mdx` **по-прежнему есть**;
- `git ls-files` → в индексе `src/components/ui/button/{Button.tsx,Button.styles.ts,Button.stories.tsx}`; `dir /b /s src` → на диске `src\Components\Ui\Button\…`; `git ls-tree HEAD …/src` → tree-запись `src/components`;
- `git config core.ignorecase` → `true`; `git -c core.ignorecase=false status --porcelain` → только `?? …/src/Components/` (с `--untracked-files=all` — три файла);
- **воспроизведение чистого клона**: `git archive --format=tar HEAD` (из корня репозитория) → распаковка `tar -x` во временную папку (имена папок получились ровно как в `HEAD`, то есть строчные) → `fsutil file setCaseSensitiveInfo …\src enable` (подтверждено `queryCaseSensitiveInfo`: «is enabled») → `mklink /J node_modules …` → `npx vite build` → **ПАДЕНИЕ, exit 1**:
  ```text
  ✗ 21 modules transformed.
  error during build:
  Could not resolve "../Components/Ui/Button/Button" from "src/views/calculator-view.tsx"
  ```
- **лечение A проверено там же**: в этой регистрозависимой копии заменена одна строка импорта на `"../components/ui/button/Button"` → `npx vite build` → **exit 0, 1106 модулей**;
- **`git add -A` НЕ лечит** — проверено на искусственно созданном репозитории-двойнике (распаковка `git archive`, `git init`, `git add .`, seed-коммит, затем `ren` папок в `Components/Ui/Button`): после `git add -A` в `git ls-files src` остались строчные пути;
- **лечение B проверено**: `git -c core.ignorecase=false rm -r --cached src` + `git add src` → `git ls-files src` стал `src/Components/Ui/Button/…`, `git -c core.ignorecase=false status --porcelain` показал три `R` (rename), после коммита новый `git archive` → регистрозависимая папка → `npx vite build` → **exit 0, 1106 модулей**;
- поведение `CalculationModel` + `CalculatorViewModel` прогнано в Node: `npx esbuild .qwen\tmp\probe\vm26.ts --bundle --platform=node --format=cjs` + `node` (таблицы в пунктах 2 и 6);
- лечение из пункта 2 прогнано отдельно на копии логики (таблица «после» в пункте 2);
- `node .qwen\tmp\css26.cjs` — разбор байтового порядка утилит по актуальному `dist\assets\index-CehiccHw.css` (пункт 5);
- отсутствие инструментов проверено glob'ом по `{.eslintrc*,eslint.config.*,.prettierrc*,prettier.config.*,vitest.config.*,vitest.workspace.*,README*,**/*.test.*,**/*.spec.*}` → **41 совпадение, все внутри `node_modules`**, в проекте — ноль; `if not exist node_modules\@vitejs\plugin-react` → **`NO_PLUGIN_REACT`**;
- `git cat-file -s 0dbe7f4:…\styles_suggestions.md` → **27943**; `git log --oneline --diff-filter=D -- …\styles_suggestions.md` → `de7ec8c`;
- `git show --stat -M de7ec8c` и `git show -M de7ec8c -- …/App.tsx …/Button.stories.tsx` — что реально изменилось в последнем коммите (пункт 12);
- `git check-ignore -v debug-storybook.log dist storybook-static .qwen` → все четыре игнорируются (`.gitignore:100`, `:71`, `:101`, `:1`);
- чтение всех непустых исходников и конфигов.

Во время самого обзора рабочие файлы проекта **не изменялись** (`git status` после всех прогонов — `working tree clean`). **Позже в этот же день** по вашей просьбе применён вариант A из пункта 1 и сделан коммит `235a6ed` — правки `calculator-view.tsx` и переименование папок на диске относятся уже к пост-обзорному шагу, детали в подразделе «Как это было применено». Временное удалено: `.qwen\tmp\clone26`, `.qwen\tmp\case26`, `.qwen\tmp\caseb26`, `.qwen\tmp\verify26`, `.qwen\tmp\jscheck`, `.qwen\tmp\probe`, `snap26.tar`, `snap_after.tar`, `caseB.tar`, `clonetest.log`, `fixA.log`, `caseB.log`, `verify_after.log`, `build_after.log`, `sb26.log`, `css26.cjs`; атрибут case-sensitivity со всех временных папок снят, junction'ы `node_modules` удалены через `rmdir` до удаления самих папок. Одна служебная запись вне папки проекта (`\.qwen_imports_check.txt` в корне репозитория) создана и удалена мной — `git status` чистый. `.qwen\tmp\mbx` не трогал (он был до меня).

Честное предупреждение, как раньше: прогоны `vite build` и `build-storybook` перезаписали `dist\` и `storybook-static\` — gitignore-нутые артефакты, пересоздаются вашими же скриптами. И ещё: в двух проверочных сборках внутри scratch-папок CSS показался как 62,89 kB вместо 8,17 kB — это артефакт замера (Tailwind просканировал смонтированный через junction `node_modules`), на ваш проект и на вывод пункта 5 это не влияет; там использован настоящий `dist\assets\index-CehiccHw.css` (8172 байт).

---

## 1. Критично: `HEAD` противоречит сам себе — чистый клон не собирается

> **РЕШЕНО 2026-10-26, вариант A, коммит `235a6ed`.** Папки приведены к строчному виду, импорт переписан, проверено: `npm run type-check` → exit 0; `npx vite build` → 1106 модулей, 945,25 kB; `git -c core.ignorecase=false status --porcelain` → пусто; `git archive HEAD` → регистрозависимая папка → `npx vite build` → **exit 0, 1106 модулей** (до правки тот же тест падал с exit 1 на 21-м модуле). Подробности и одна важная поправка к пункту 10 — ниже, в подразделе «Как это было применено».

Расхождение теперь не «индекс vs диск», а **внутри самого коммита**, и потому оно не чинится ни `git status`, ни `git add`:

| что говорит `HEAD` (и любой клон) | что пишет код в `HEAD` |
| -------------------------------- | ---------------------- |
| `src/components/ui/button/Button.tsx` | `src/views/calculator-view.tsx:5-6` → `import … from "../Components/Ui/Button/Button"` |

На вашей машине это невидимо по двум причинам сразу: `core.ignorecase = true` и NTFS без атрибута case-sensitivity. Проверено и то и другое (`git config core.ignorecase` → `true`; `fsutil … queryCaseSensitiveInfo` на обычном дереве → атрибут выключен, а во временной папке я его включал принудительно — и сборка упала).

Хронология по коммитам, чтобы было понятно, где именно соскользнуло: `86b6e6e` зафиксировал переход на `src/components/ui/button/` (в диффе видно `ui/{buttons => button}/Button.styles.ts`) — то есть строчный вариант тогда был и на диске, и в индексе. Позже папки были переименованы в `Components/Ui/Button` уже **на диске**, а вместе с ними переписали импорты — и `de7ec8c` унёс в `HEAD` именно эти новые импорты, потому что git с `core.ignorecase = true` переименование только в регистре не видит вовсе. Отсюда и `?? …/src/Components/` в выводе `git -c core.ignorecase=false status --porcelain` при полном `git status` «clean».

Практический смысл: **CI/Docker/WSL2/клона на macOS с этого коммита красные**. Локальный зелёный `npm run build` — не аргумент.

Два лечения, оба проверены прогонами. Выбирайте одно и не смешивайте.

**A. Вернуть папки в строчный вид (рекомендую: меньше всего изменений, приводит дерево к стилю `models/`, `view-models/`, `views/`, который у вас уже принят).** Переименование только в регистре на Windows делается в два шага, иначе `ren` отказывает:

```cmd
cd /d C:\other\SelfEducation\js\react\20_practiceCalculator\src
ren Components Components.tmp
ren Components.tmp components
cd components
ren ui ui.tmp
ren ui.tmp ui
cd ui
ren button button.tmp
ren button.tmp button
```

После этого `git -c core.ignorecase=false status --porcelain` должен стать **пустым** (содержимое файлов и так совпадает с индексом — расхождение только в имени пути). Затем одна правка в коде — импорт в нижнем регистре, заодно снимает дубль из пункта 3. `src/views/calculator-view.tsx`, первые шесть строк:

```tsx
import { observer } from "mobx-react-lite";
import type { CSSProperties } from "react";
import { CalculatorViewModel } from "../view-models/calculator-view-model";
import { Button, ButtonType } from "../components/ui/button/Button";
```

Проверено: ровно этот вариант в регистрозависимом клоне даёт `vite build` exit 0 / 1106 модулей.

**B. Оставить PascalCase и записать его в git** — если вы сознательно хотите `Components/Ui/Button`. `git add -A` это **не** чинит (проверено выше); нужен перестрой индекса:

```cmd
cd /d C:\other\SelfEducation\js\react\20_practiceCalculator
git -c core.ignorecase=false rm -r --cached src
git add src
git -c core.ignorecase=false status --porcelain
git commit -m "Record component folder casing (fixes clean-clone build)"
```

`rm --cached` рабочий дерево не трогает. Проверено: `git ls-files src` после recipe показывает `src/Components/Ui/Button/Button.tsx`, статус — три `R`, а новый клон из полученного коммита собирается на регистрозависимой ФС (exit 0). Минус варианта B: три верхних папки останутся в PascalCase рядом с `models/`, `view-models/`, `views/` в нижнем — расхождение стиля из пункта 7 так и продолжит жить.

В любом варианте добавьте тот тест, которым это ловится, — он стоит одну строку и работает без запуска сборки:

```cmd
git -c core.ignorecase=false status --porcelain
```

непустой вывод = индекс и диск разошлись по регистру. В CI — `git -c core.ignorecase=false status --porcelain || exit 1` и проверка, что вывод пуст.

### Как это было применено (2026-10-26, вариант A, коммит `235a6ed`)

Применение упёрлось в неочевидное препятствие: **переименование папок отказывало с `Отказано в доступе`**. Диагноз по трём пробам:

- переименование файла внутри `src\Components\Ui\Button` (`Button.styles.ts` → `.tmp`) — **проходит**;
- переименование несвязанной папки `src\views` → `views.tmp` — **проходит**;
- переименование `src\Components` и `src\Components\Ui` — **отказывается**, и `Rename-Item` даёт тот же `System.IO.IOException: Access to the path … is denied`.

Значит блокируется не ФС и не атрибуты (проверено: `fsutil file queryCaseSensitiveInfo` на обеих папках → «is disabled», `dir /a` без необычных атрибутов), а **открытый handle на саму папку**. Владельцы — языковые серверы Zed (`Get-CimInstance Win32_Process` по `node.exe` показал 11 процессов: eslint, tailwindcss-language-server, vtsls + два tsserver + typingsInstaller, emmet, prettier — и ни одного dev-сервера). Watcher держит handle на `src\Components\Ui\Button`, поэтому переименовать можно файл внутри, но не родительскую папку. После остановки этих процессов (с разрешения владельца машины; `qwen`-процессы не трогались) две двухшаговые `ren` прошли, и `src` стал `components/ui/button`.

Важное следствие, которого я не предвидел: **между правкой импорта и переименованием папок `npm run type-check` стал красным** и показал то, что два прошлых прохода считали невидимым:

```text
src/components/ui/button/Button.tsx:7:8 - error TS1261:
  Already included file name '…/src/components/ui/button/Button.styles.ts'
  differs from file name '…/src/Components/Ui/Button/Button.styles.ts' only in casing.
src/views/calculator-view.tsx:4:36 - error TS1149 / TS1261 (то же про Button.tsx)
Found 3 errors in 3 files.
```

То есть `forceConsistentCasingInFileNames` **не бессилен** — он ловит расхождение «импорт vs диск» как только стороны разойдутся. Он бессилен ровно в одном: расхождение «индекс/HEAD vs диск» он не видит никогда, потому что tsc работает с ФС, а не с git. Это поправка к пункту 10 и к формулировке прошлого отчёта.

Итоговые проверки после применения (все — реальные прогоны):

```text
npm run type-check            -> exit 0
npx vite build                -> 1106 модулей, JS 945,25 kB (gzip 278,03), CSS 8,27 kB
git status --short            -> пусто (кроме этого отчёта)
git -c core.ignorecase=false status --porcelain -> пусто   <-- расхождение исчезло
git archive HEAD → распаковка → fsutil setCaseSensitiveInfo src enable → npx vite build
                              -> exit 0, 1106 модулей   <-- до правки: exit 1 на 21-м модуле
```

Побочный эффект диагностики, о котором честно говорю: два пробных переименования (`Button.styles.ts` и `src\views`) реально выполнялись по рабочему дереву и были **немедленно откачены обратно** (`Button.styles.ts`, `views`); финальный `git status` не содержит ни их, ни следов — дерево чистое, в коммит попал только `calculator-view.tsx`. Имена файлов внутри папки (`Button.tsx`, `Button.styles.ts`, `Button.stories.tsx`) не менялись: они и в индексе, и на диске были с заглавной, расходились только имена **папок**.

## 2. `(2+3` показывает `Error`: незакрытая скобка не считается незавершённым вводом

Проверено прогоном актуальных `CalculationModel`/`CalculatorViewModel` в Node (mobx 7.0.6 + mathjs 15.2.0). Текущая `INCOMPLETE_TAIL = /[+\-*/.(]\s*$/` смотрит **только последний символ**:

```text
"(2+3"     !ok invalid      visible="Error"     <-- баг: пользователь в середине ввода
"((1+2)"   !ok invalid      visible="Error"     <-- баг
"(1+2"     !ok invalid      visible="Error"     <-- баг
"2+"       !ok incomplete   visible=""
"2++"      !ok incomplete   visible=""
"2*3("     !ok incomplete   visible=""          <-- здесь хвост как раз "(" и срабатывает
"2+()"     !ok invalid      visible="Error"     <-- верно
"1/0"      !ok not-a-number visible="Error"     <-- верно
"0/0"      !ok not-a-number visible="Error"     <-- верно
"1..2"     !ok invalid      visible="Error"     <-- верно
"2+3)"     !ok invalid      visible="Error"     <-- верно
")"        !ok invalid      visible="Error"     <-- верно
"()"       !ok invalid      visible="Error"     <-- верно
```

То есть прошлое лечение сработало ровно для одного случая — оборванного оператора на конце. Скобки при этом живут в самом начале клавиатуры (row 1, column 1-2), и `(2+3` — это нормальный промежуточный этап набора `(2+3)*4`, а не ошибка. Полный прогон сессии набора это подтверждает: `2` → `2`, `+` → `""`, `3` → `5`, `*` → `""`, `4` → `14` — а с открывающей скобкой та же сессия печатает `Error` до тех пор, пока скобка не закрыта.

Лечение — считать незавершённым и ввод с неотбалансированными скобками. Прогнано на копии, таблицу «до/после» см. выше: спорные случаи уходят в `""`, настоящие ошибки остаются `Error`.

`src/models/CalculationModel.ts` — файл целиком:

```ts
import { evaluate as mathEvaluate } from "mathjs";

const INCOMPLETE_TAIL = /[+\-*/.(]\s*$/;

export type EvaluationResult =
  | { ok: true; display: string }
  | { ok: false; reason: "empty" | "incomplete" | "invalid" | "not-a-number" };

function isIncomplete(input: string): boolean {
  if (INCOMPLETE_TAIL.test(input)) {
    return true;
  }
  let depth = 0;
  for (const char of input) {
    if (char === "(") {
      depth++;
    } else if (char === ")") {
      depth--;
    }
  }
  return depth > 0;
}

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
      return isIncomplete(input)
        ? { ok: false, reason: "incomplete" }
        : { ok: false, reason: "invalid" };
    }
  }
}
```

`CalculatorViewModel` и `View` менять не нужно: ветка `incomplete` уже ведёт себя как `empty` (`result` → `""`).

Плюс к этому, раз `reason` теперь реально читается в четырёх значениях, подумайте над разницей между `invalid` и `not-a-number`: сегодня оба рисуют `Error`, и `union` на этом этапе оправдан только `empty`/`incomplete`. Второй вариант — схлопнуть `not-a-number` в `invalid`, а `Error` оставить единственно возможным текстом ошибки.

## 3. Два импорта из одного модуля и `import` типа как значение

`src/views/calculator-view.tsx:5` и `:6` — две строки импорта **из одного и того же модуля** (`../Components/Ui/Button/Button`): сначала `Button`, затем `ButtonType`. Работает, но это удвоение, которое ESLint (`import/no-duplicates`) сливает в одну строку, а читатель спотыкается: «это, может, разные файлы?» Кроме того, `CalculatorViewModel` в этом файле используется **только в позициях типа** (в `interface CalculatorViewProps` и в дженерике хелперов) — при `isolatedModules` для него просится `import type`. То же в `src/App.tsx:1`.

`src/views/calculator-view.tsx`, первые четыре строки (после варианта A из пункта 1):

```tsx
import { observer } from "mobx-react-lite";
import type { CSSProperties } from "react";
import type { CalculatorViewModel } from "../view-models/calculator-view-model";
import { Button, ButtonType } from "../components/ui/button/Button";
```

`src/App.tsx` — файл целиком:

```tsx
import type { CalculatorViewModel } from "./view-models/calculator-view-model";
import { CalculatorView } from "./views/calculator-view";

interface AppProps {
  viewModel: CalculatorViewModel;
}

export default function App({ viewModel }: AppProps) {
  return <CalculatorView viewModel={viewModel} />;
}
```

`src/main.tsx` менять не нужно — там `CalculatorViewModel` вызывается через `new`, это настоящее значение. В `Button.tsx` та же мелочь: `import { CSSProperties } from "react"` — импорт типа как значения (`Button.tsx:1`), и `React.FC` при неимпортированном `React` (`Button.tsx:33`, работает за счёт UMD-глобала из `@types/react`, `tsc` принимает — проверено exit 0). Либо `import type { CSSProperties, FC } from "react"`, либо убрать `FC` в пользу сигнатуры функции — но последовательно.

## 4. Мелкая логика, которая всё ещё не решена: дырка в сетке и фиолетовая точка

Перенос из прошлого отчёта, не исправлено. `keys` — 19 элементов, сетка `repeat(4, 1fr) / repeat(5, 1fr)` — 20 ячеек; клетка `(row 5, column 3)` пуста, соседи: `0` (5/1), `.` (5/2), `+` (5/4). И `label: "."` по-прежнему размечен как `ButtonType.Operator` → пурпурная, хотя точка — часть числа, а не оператор; рядом свободный `ButtonStyle.Digit`.

`equals()` из ViewModel удалён, клавиши `=` нет — выбор «live-preview вместо фиксации результата» принят и мёртвого кода больше нет, это законно. Но тогда дырка в сетке — это не «свободное место», а следствие удалённой кнопки. Закрывается либо `colSpan` у `0` (тогда в `ButtonProps` нужен проп под `col-span`, а в стилях — `col-span-(--span)`), либо перестановкой `+` на третью колонку и `.` — на четвёртую, либо возвращением `=`.

## 5. Перенос из двух прошлых отчётов, не исправлено: оверрайд `bg-*` из `className` молча проигрывает

`Button.tsx:44-46` (`[buttonTypeStyles[buttonType], className]`) не менялся. Перемерил порядок на **актуальном** `dist\assets\index-CehiccHw.css` (8172 байта), скриптом:

```text
bg-blue-500@5582    bg-cyan-500@5634    bg-green-500@5686
bg-purple-500@5740  bg-red-500@5796
text-3xl@5937       text-lg@6032
text-black@6212     text-white@6249     shadow-md@6322
победитель: bg-cyan-500 vs bg-red-500 -> WINS bg-red-500
победитель: text-white  vs text-black  -> WINS text-white
```

Механика та же: в одном свойстве Tailwind раскладывает утилиты по алфавиту значения, а не по порядку в атрибуте `class`, поэтому «последний класс побеждает» здесь не работает. Следствие: история `CustomClassName` и любой оверрайд цвета цифры через `className` (`bg-cyan-500` против `bg-red-500`) **не сработает** — ни ошибки, ни предупреждения; `text-white` побеждает только потому, что `white` алфавитно позже `black`. Это тот же класс тихой ошибки, что и регистр папок: ни `tsc`, ни консоль не ругаются.

Без новых зависимостей: не перекрывать в `className` то свойство, которое задал `buttonType` (а историю `CustomClassName` переписать на `text-white` → `shadow-md`, то, что не конфликтует). С зависимостью: `tailwind-merge`, `twMerge([buttonTypeStyles[buttonType], className].filter(Boolean).join(" "))`.

Отдельно по `Button.styles.ts` — там появился новый порядок (`buttonBase` + `withColor(color)`), и он хороший, но имена экспортов разошлись с enum: `buttonNumber` → для `ButtonType.Digit`, `buttonManage` → для `ButtonType.Action`. Две терминологии для одного понятия в пределах двух строк одного файла; в `buttonTypeStyles` это читается как шифр. Если решите чинить имена (пункт 7), то вот файл целиком:

```ts
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

export const buttonDigit = withColor("bg-red-500");
export const buttonOperator = withColor("bg-purple-500");
export const buttonAction = withColor("bg-green-500");
export const buttonBracket = withColor("bg-blue-500");
```

и соответствующая правка в `src/components/ui/button/Button.tsx`:

```tsx
import {
  buttonBracket,
  buttonAction,
  buttonDigit,
  buttonOperator,
} from "./Button.styles";

// …

const buttonTypeStyles: Record<ButtonType, string> = {
  [ButtonType.Digit]: buttonDigit,
  [ButtonType.Operator]: buttonOperator,
  [ButtonType.Action]: buttonAction,
  [ButtonType.Bracket]: buttonBracket,
};
```

## 6. `mathjs`: что даёт эта клавиатура (проверено перебором через ваш класс)

Прогнал через актуальный `CalculationModel`:

```text
"2++3"                    ok  5        // унарный плюс
"7--2"                    ok  9
"2(3)"                    ok  6        // неявное умножение
"2*-3"                   ok  -6
"-5"                     ok  -5
"2."                     ok  2         // висячая точка проглатывается
"007"                    ok  7
"999999999999999999999"  ok  1e+21
"1/0"                    !ok not-a-number
"0/0"                    !ok not-a-number
"1..2"                   !ok invalid
"2*/3"                   !ok invalid
"1 2"                    !ok invalid
"5!"                     ok  120       // "!" на клавиатуре нет — недостижимо
"2^-1"                   ok  0.5       // "^" на клавиатуре нет — недостижимо
```

Guard'ы по типу и `Number.isFinite` закрывают всё достижимое (`Complex`/`Unit` с этой клавиатуры не набрать), так что в UI они не протекают. Whitelist всё равно нужен, но по другой причине: `2++3` → `5` и `2(3)` → `6` — это не «безопасно», это другой калькулятор; `999999999999999999999` → `1e+21` — это научная нотация в поле, где её быть, вероятно, не задумано. Одна проверка на границе, перед `mathEvaluate`:

```ts
const ALLOWED_INPUT = /^[\d+\-*/(). ]+$/;
```

Эффект перехода на `mathjs/number` (прошлое измерение: 945,24 kB → 654,22 kB, 1105 → 718 модулей) в этом прогоне **не перемерялся**; честная цифра сегодня: `npx vite build` → 945,25 kB JS / 1106 модулей, то есть с прошлой ревизии вес не изменился и пункт остаётся открытым.

## 7. Именование: что не сошлось после переименований

Три разных стиля живут в одном `src` одновременно:

| сейчас | что с этим не так | как принято |
| ------ | ----------------- | ----------- |
| `src/Models`→`src/models`, `view-models`, `views` — строчные, а `src/Components/Ui/Button` — PascalCase | расхождение не только стиля, но и регистра в git (пункт 1) | один стиль на всё дерево: `components/ui/button/` |
| `models/CalculationModel.ts` (`class CalculationModel`) vs `view-models/calculator-view-model.ts` (`class CalculatorViewModel`) | `Calculation` vs `Calculator` — два корня для одной сущности, и именно в соседних слоях MVVM | `calculation-model.ts` оставить (он про «вычисление»), но тогда в VM поле/именование тоже про действие: `evaluator` ✓ (тут уже хорошо) |
| метод VM `appendChar` vs хелпер вью `pressAppend` | два имени для одной операции; `pressClear`/`pressBackspace` уже совпадают, а `append` — нет | либо `pressAppend`+`pressClear`+`pressBackspace` в VM, либо хелпер `appendChar` в вью |
| `const keys` и `key={key.label}` при `interface KeypadButton` | массив называется `keys`, элемент — `KeypadButton`, а React-пропс — тоже `key`; метка как ключ завязана на её уникальность | `const keypad` / `keypad.map((button) => …)` / `key={button.id}` |
| `Button.stories.tsx:4` — `title: "UI/CalculatorButton"` | не совпадает ни с именем компонента (`Button`), ни с путём (`components/ui/button`) | `"Components/Button"` |
| `Button.stories.tsx:49-54` — `export { NumberKey as Number, ManageKey as Manage, … }` | в сайдбаре истории зовутся `Number`/`Manage`, а enum — `Digit`/`Action`; прошлый комментарий, объяснявший алиасы, удалён в `de7ec8c`, и теперь алиасы выглядят случайными | объявить `export const Digit: Story`, `Action: Story` прямо, без ре-экспорта |
| `src/index.css` — одна строка `@import "tailwindcss";` | ок, но тогда у проекта нет ни одного собственного класса: вся стилистика в утилитах | ничего не делать |

## 8. История: `styles_suggestions.md` удалён, и это потеря полезного

`git log --oneline --diff-filter=D -- …\styles_suggestions.md` → `de7ec8c`. Файл был 0 байт (обнулили раньше, см. прошлый отчёт, пункт 11), но содержимое живёт в истории: `git cat-file -s 0dbe7f4:…\styles_suggestions.md` → **27 943 байта**. Удаление пустого файла — решение разумное, просто имейте в виду, что вместе с ним вы из рабочего дерева убрали последнюю ссылку на большой разбор стилей:

```cmd
git show 0dbe7f4:js/react/20_practiceCalculator/styles_suggestions.md > styles_suggestions.md
```

Отчёты `2026-10-25 suggestions.md` и `2026-10-25_1 suggestions.md` в индексе есть ✓ (прошлое замечание «вся ревизия только в истории» закрыто). `.qwen/`, `dist`, `storybook-static`, `debug-storybook.log` игнорируются ✓ (проверено `git check-ignore -v`).

Про состав коммита `de7ec8c "Some changes"`: восемь файлов разношёрстного — правка опечатки `AddProps`, переименование класса в `CalculatorViewModel`, новая ветка `incomplete`, переписанные импорты в вью (те самые, что сломали клон), удалённый комментарий в историях, удалённый пустой `styles_suggestions.md` и правка собственного отчёта. Регистровую правку из пункта 1 удобно выделять в отдельный коммит — она единственная меняет путь в дереве.

## 9. Перенос: инструменты куплены и не используются

Проверено glob'ом и `if not exist` (результаты — в секции «Как проверялось»): в проекте **ноль** тестовых файлов, ноль конфигов ESLint/Prettier, нет `README`, нет `vitest.config.*`, нет `test`-скрипта, `@vitejs/plugin-react` **отсутствует** в `node_modules`. Следствия:

- `@storybook/addon-vitest` + `vitest` + `@vitest/browser-playwright` + `@vitest/coverage-v8` + `playwright` установлены и зарегистрированы, но запускать нечего; `playwright` тянет скачивание браузеров. Либо 3–4 теста на `CalculationModel.evaluate` — то, что уже доказано прогонами в пунктах 2 и 6 (`empty`/`incomplete`/`invalid`/`not-a-number`, `toPrecision(12)`, скобки), — либо весь блок удалить. Прошлая проверка показывала: `npx vitest run …` стартует без единого конфига, достаточно `"test": "vitest run"` в `scripts`.
- Нет Fast Refresh: правка `Button.tsx` перезагружает страницу и **сбрасывает `expression`** — ровно то состояние, которое в учебном проекте вы учитесь наблюдать. Лечится `@vitejs/plugin-react` в `vite.config.mjs`.
- `.storybook/main.mjs:6` — паттерн `"../src/**/*.mdx"` не находит файлов, `build-storybook` печатает предупреждение (проверено в этом прогоне). Убрать строку или добавить `docs.mdx`.
- `package.json`: `"type": "commonjs"` при полностью ESM-коде, `"private": true` нет (`npm publish` по ошибке выложит `20_practicecalculator` в реестр), `"description"`/`"keywords"`/`"author"` пустые, `"license": "ISC"` для учебного private-проекта.

## 10. `tsconfig.json`: `.storybook` в `include` ничего не проверяет

`tsconfig.json:20` — `"include": ["src", ".storybook"]`, при этом `allowJs` не включён (дефолт `false`). Значит `.storybook/main.mjs` и `.storybook/preview.mjs` в компиляцию **не попадают**: строка `include` создаёт впечатление, что конфиги проверены типами, а их никто не читает. Две честные развязки: либо убрать `".storybook"` из `include`, либо добавить `"allowJs": true` (тогда `checkJs` не включать — иначе начнут сыпаться ошибки на JSDoc-аннотации `@type` в этих файлах).

Проверено прямым опытом на копии проекта: в копию с тем же `tsconfig.json` положен `.storybook/main.mjs` с заведомыми ошибками (`const broken: number = "…"` — TS-аннотация в `.mjs`, обращение к несуществующему свойству, `map()` без аргумента) и, для контроля, `src/control.ts` с `export const Control: number = "deliberately wrong on purpose"`. Результат:

```text
npx tsc --noEmit -p …\jscheck\tsconfig.json   (ccontrol.ts удалён)  -> exit 0, ошибок нет
npx tsc --noEmit -p …\jscheck\tsconfig.json   (ccontrol.ts добавлен) -> exit 1
  src/control.ts:1:14 - error TS2322: Type 'string' is not assignable to type 'number'.
```

То есть `src` этим конфигом проверяется, а `.storybook/*.mjs` — нет: `allowJs` по умолчанию `false`, и tsc эти файлы даже не открывает. Строка `"include": [".storybook"]` сейчас ровно пустая decorative-запись, и полагаться на неё как на проверку конфигов нельзя.

Полезные флаги, которых не хватает под учебный проект: `"noUncheckedIndexedAccess": true` (ловит `buttonTypeStyles[buttonType]`-подобные обращения к `Record` по индексу и `arr[i]`), `"exactOptionalPropertyTypes": true` (для `disabled?: boolean` + `className?: string`), `"noPropertyAccessFromIndexSignature": true`.

Отдельно про `"forceConsistentCasingInFileNames": true` — и **поправка к пункту 1 этого отчёта и к формулировке прошлого отчёта**. Раньше я писал, что tsc «не видит этого в принципе». Сегодня это опровергнуто наблюдением: когда импорт стал строчным, а папки на диске ещё были PascalCase, `npm run type-check` покраснел тремя `TS1149`/`TS1261` («File name … differs from already included file name … only in casing»). То есть флаг **ловит** расхождение «импорт vs реальный путь на диске» — и именно он не дал мне оставить дерево в полупримененном состоянии. Ограничение у него ровно одно, и оно про другой случай: расхождение «`HEAD`/индекс vs диск» он не видит никогда, потому что tsc работает с файловой системой, а не с git. Пока импорт и диск соглашались оба в PascalCase (состояние `de7ec8c`), tsc был зелёным при неработающем клоне — см. пункт 1. Формально флаг включён по умолчанию в современных `tsc`, но прописать его явно стоит: он дешёвый и он реально сработал.

## 11. Перенос: доступность

Не проверено скринридером (Browser Use на этой машине не работает, a11y-аддон я не запускал) — стандартные паттерны, а не измерение:

- `⌫` и `CE` по-прежнему без `aria-label` (grep по `src` на `aria-label` → 0 совпадений), а в `ButtonProps` пропа под него нет вовсе, хотя `label` — визуальный текст: озвучка `⌫` зависит от голоса, `CE` читается по буквам;
- подпись `Result:` лежит **внутри** live-зоны, поэтому при каждом нажатии перечитывается и она, и значение:

```tsx
      <p className="text-3xl font-bold">Expression: {viewModel.expression}</p>
      <p className="text-3xl font-bold">
        Result: <output aria-live="polite">{viewModel.result}</output>
      </p>
```

- `<output>` уже имеет неявный `role="status"` (он и так polite) — явный `aria-live` дублирует, без вреда;
- клавиатура не работает с клавиатуры же: фокус есть (это настоящие `<button>`), но нет никакого обращения к `expression` через настоящий ввод. Для учебного проекта это не обязательное, но характерное улучшение: `<input>`-ловушка или обработчик `keydown` на контейнере.

## 12. Что исправилось с прошлой ревизии (проверено)

`npm run type-check` зелёный: `meta.args` получил `buttonType`, `TS2322` ушёл ✓. `AddProps` → `AppProps` ✓. `class ViewModel` → `class CalculatorViewModel`, и имя файла (`calculator-view-model.ts`) теперь совпадает с именем класса ✓. `CalculationViewProps` → `CalculatorViewProps` ✓. `EvaluationResult.reason` читается (`empty` → `""`) и стартовый экран больше не `Error` ✓. `makeAutoObservable(this, { evaluator: false })` — ровно тот вариант, что предлагался ✓. `Button.styles.ts` в индексе и собирается ✓. `src/app.tsx` → `src/App.tsx` в индексе (прошлый Critical по этой паре закрыт) ✓. `equals()` убран вместе с кнопкой, мёртвого кода нет ✓. Стили собраны через `buttonBase` + `withColor` вместо четырёх копий списка ✓. `.mdx`-предупреждение, `reason` для незакрытой скобки, оверрайд цвета, дырка в сетке, `mathjs/number`, инструменты и Fast Refresh — остались открытыми.

## 13. Порядок правок

1. ✓ **Выполнено 2026-10-26, коммит `235a6ed`** (пункт 1, вариант A): три двушаговых `ren` + одна строка импорта. Проверено: `git -c core.ignorecase=false status --porcelain` пуст, `npm run type-check` exit 0, регистрозависимый клон собирается (1106 модулей вместо падения на 21-м). Вариант B (`rm -r --cached src` + `git add src`) так же проверен, но остался не выбранным.
2. Пункт 2: `isIncomplete()` с балансом скобок в `CalculationModel.ts` → `Error` перестаёт быть штатным состоянием при наборе `(2+3)*4`.
3. Пункт 3: слить два импорта в один, `import type` там, где тип используется только как тип (`calculator-view.tsx`, `App.tsx`, `Button.tsx`).
4. Пункт 4: решение по сетке (вернуть `=` / `colSpan` / переставить `+` и `.`) и перекрасить `.` в `Digit`.
5. Пункт 5: не перекрывать `bg-*` из `className`, либо `tailwind-merge`; заодно выровнять имена `buttonNumber`/`buttonManage` → `buttonDigit`/`buttonAction`.
6. Пункт 6: whitelist входных символов; затем `import { evaluate } from "mathjs/number"` и сверить с 945,25 kB (ожидание ≈654 kB по прошлому замеру).
7. Пункт 7: один стиль имён папок на всё дерево + `title` историй + убрать алиасы `Number`/`Manage`.
8. Пункт 10: `.storybook` из `include` убрать (или `allowJs`), добавить `noUncheckedIndexedAccess`.
9. Пункт 9: тесты на `evaluate` (2–4 штуки) + `"test": "vitest run"`, либо удалить vitest/playwright-блок; `@vitejs/plugin-react` для Fast Refresh; ESLint + Prettier (иначе пункты 3 и 7 вернутся через неделю); `"private": true`; `.mdx`-паттерн; README.
10. Пункт 8: решить судьбу отчётов (восстановить `styles_suggestions.md` из `0dbe7f4` или смириться с удалением) и коммитить шаг за шагом, регистровую правку — отдельно.

## 14. Что не проверялось

- Поведение в браузере: скриншотов/кликов не было. Все выводы по UI — из прогонов `ViewModel`/`mathjs` в Node и из чтения кода.
- Точный эффект `mathjs/number` в этом прогоне (не пересобирал копию с изменённой строкой импорта; цитирую прошлое измерение как прошло).
- Доступность скринридером; a11y-аддон не запускался.
- `npm run build-storybook` зелёный, но сами истории в браузере не открывал; после правок `Button.styles.ts`-имён истории не пересматривались.
- ~~Что вариант A (`ren` в нижний регистр) не оставляет артефактов вроде пустой папки `Components`~~ — **проверено после применения**: `dir /b /s src` показал только `components\ui\button` со всеми тремя файлами, без следа `Components` и без `*.tmp`; `git -c core.ignorecase=false status --porcelain` — пусто. Остаток в виде `src\Components.tmp`, который мог бы появиться при обрыве двухшагового переименования, тоже исключён.
- Порядок утилит Tailwind из пункта 5 **не перемерялся** на новом `dist\assets\index-BGZAOrYY.css` (8,27 kB), полученном после переименования папок: вывод пункта 5 снят с `index-CehiccHw.css` (8172 байт) до правки. Состав классов и версия Tailwind не менялись, поэтому порядок алфавита значения сохранится, но это вывод из неизменности входных данных, а не второе измерение.
- `checkJs`-сценарий из пункта 10 (что именно сломается, если включить `allowJs` + `checkJs` в `.storybook`) не прогонялся.
