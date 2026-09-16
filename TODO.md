# CrispLEDES releases

## CrispLEDES 1.5.9 release

- [x] Preserve storyboard-visible Cocoa classes in Release builds.
- [x] Align project, assembly, file, and bundle versions at 1.5.9.
- [x] Build, sign, notarize, staple, and validate the public 1.5.9 package (SHA-256 `9c6758b8f1bbf7dc44841b35fe4ae1a0899dc61a776fd61faa2cbd9009a6b52c`).
- [x] Verify the package contains only the sanitized public configuration.
- [ ] Commit and push the release changes to `main`.
- [ ] Publish `v.1.5.9` from the exact `main` commit with the verified package asset.

## CrispLEDES 1.5.8 release

## Fresh local Release rebuild (2026-09-15)

- [x] Inventory and stop running CrispLEDES instances.
- [x] Remove confirmed local app bundles and generated build outputs.
- [x] Restore and build the macOS app in Release.
- [x] Correct Release registrar preservation for storyboard-only Cocoa classes.
- [x] Launch the fresh Release build and verify that it stays running.

- [x] Reconcile and commit the existing macOS/MAUI project split.
  - [x] Upgrade the MAUI SQLite dependency closure to remove the high-severity `SQLitePCLRaw.lib.e_sqlite3` 2.1.10 advisory.
- [x] Align the production macOS project with .NET 10 and version 1.5.8 package, assembly, and file stamps.
  - [x] Align the hard-coded bundle short/build versions in `Info.plist` at 1.5.8.
  - [x] Make the vendored lifecycle reject app bundles whose embedded versions do not match the project version.
- [x] Synchronize null-safety and macOS asset-catalog fixes with the private codebase.
- [x] Vendor the public-safe macOS lifecycle from ROI.BuildActions 1.1.3 without private feed or secret-injection logic.
- [x] Restore and build the production macOS project with zero warnings/errors.
- [x] Build, sign, notarize, staple, and validate the public 1.5.8 package (SHA-256 `4244f56abe38fe625c2684d879a7e194fd141d756de479c2f11f329091466872`).
- [x] Verify that the package contains the sanitized public configuration and no private configuration.
- [x] Commit and push separate structural and release-packaging changes to `main`.
- [x] Publish `v.1.5.8` from exact `main` commit `80472348528b5d55041a7107c4166611a68ff7d4` with the verified package asset.
- [x] Keep the public repository excluded from JAMF; only the private package is MDM deployable.
- [x] Mark the public repository metadata as not deployable through MDM.
- [x] Restore the standard `rol-profile` default in the public-safe vendored Build Actions lifecycle.
