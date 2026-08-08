# Repository Guidelines

## Project Structure & Module Organization
- `CrispLEDES/` is the .NET MAUI app (cross-platform UI). Core pages live in `CrispLEDES/Pages/`, view models in `CrispLEDES/PageModels/`, data access in `CrispLEDES/Data/`, and shared models in `CrispLEDES/Models/`.
- `src/` contains the legacy macOS-specific app and converter logic (e.g., `src/Converter/`, `src/MainViewController.cs`).
- Shared assets live under `CrispLEDES/Resources/` (fonts, styles, app icons) and `src/Assets.xcassets/` for the macOS app.
- Reference docs and sample configuration live in `README.md` and `src/configuration-file.txt`.

## Build, Test, and Development Commands
- `dotnet build CrispLEDES/CrispLEDES.csproj` builds the MAUI app (ensure MAUI workload installed).
- `dotnet run --project CrispLEDES/CrispLEDES.csproj -f net10.0-maccatalyst` runs the MAUI app on macOS.
- `dotnet build src/CrispLEDES.sln` builds the macOS app solution.
- `dotnet run --project src/CrispLEDES.Mac.csproj` runs the macOS app.

## Coding Style & Naming Conventions
- Indentation: 4 spaces; use C# keywords (`string`, `int`) over BCL types.
- Prefer `var` when the type is clear from the right-hand side.
- Private fields use `_` prefix (e.g., `_repository`).
- Nullable reference types and implicit usings are enabled; keep nullability warnings clean.

## Testing Guidelines
- No automated test project is currently present. If adding tests, prefer xUnit and place them under a new `test/` directory.
- Name tests with clear behavior-focused phrases (e.g., `ConvertInvoice_EmptyLines_Ignored`).

## Commit & Pull Request Guidelines
- Commit history uses short, sentence-case summaries and occasional version tags (e.g., `v. 1.5.6`). Follow that pattern.
- PRs should include a brief summary, the scope (MAUI vs macOS app), and screenshots for UI changes.

## Security & Configuration Tips
- Do not commit real client data or invoices. Use sanitized CSVs and the sample config in `src/configuration-file.txt` as a template.
- Prefer logging over console output for app diagnostics.
