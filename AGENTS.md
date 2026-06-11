# Project Coding Instructions

## C# Style

- Use simple `using` directives for namespaces.
- Do not replace project type names with aliases just to avoid naming conflicts.
- Keep original class, field, and type naming unless the user explicitly asks to rename something.
- Write field and property attributes on the same line as the declaration.
- Do not bind scene components in installers. Scene components are bound through the `ZenjectBinding` component.
- Do not use Unity coroutines. Use UniTask for async delays, loops, and fire-and-forget async flows.
- For second-based UniTask delays, use `UniTask.WaitForSeconds` instead of `UniTask.Delay`.

```csharp
[Inject] private RocketState _state;
```
