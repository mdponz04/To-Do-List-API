# Go Naming Conventions

| Element | Go Convention | Example |
|---|---|---|
| Folder | Lowercase, short, preferably one word | `repository/`, `postgresql/` |
| File | Lowercase, words separated by `_` when needed | `task_repository.go` |
| Package | Lowercase, short, usually one word | `repository`, `repo` |
| Struct (type) | `PascalCase` | `TaskRepository`, `PostgresTaskRepository` |
| Interface | `PascalCase`, usually describes behavior | `TaskRepository` |
| Function/method | `PascalCase` if exported; `camelCase` if private | `NewTaskRepository`, `getByID` |
| Variable/parameter | `camelCase` | `taskRepo`, `db` |
| Constant | Usually `PascalCase` if exported; otherwise `camelCase` | `DefaultPageSize` |
| Acronym | Keep common initialisms uppercase | `ID`, `HTTP`, `URL`, `DB` |

## Additional Notes

- **Exported identifiers** start with an uppercase letter and can be accessed from other packages.
- **Unexported identifiers** start with a lowercase letter and are only accessible within the same package.
- Go does not use an `I` prefix for interfaces. Prefer `TaskRepository` over `ITaskRepository`.
- Go does not have classes. Use `struct` types and methods to define data and behavior.
- Package names should be short and meaningful. Avoid redundant prefixes.
- File names and package names do not have to match, but consistency improves readability.

- **Pointer`*`and address`&`**:<br>
example: Use `&` to get the address of a variable, use `*` to dereference a pointer and access the value at its address.

```
x := 10
p := &x // p is pointer holding address of x
fmt.Println(*p) // *p is value inside the address that p point to
*p = 20
fmt.Println(x)
```
--------------------------
- **using `_` to ignore the return value that dont use** 