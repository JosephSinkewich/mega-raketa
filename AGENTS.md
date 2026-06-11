# Project Coding Instructions

## C# Style

- Use simple `using` directives for namespaces.
- Do not replace project type names with aliases just to avoid naming conflicts.
- Keep original class, field, and type naming unless the user explicitly asks to rename something.
- Write field and property attributes on the same line as the declaration.

```csharp
[Inject] private RocketState _state;
```
