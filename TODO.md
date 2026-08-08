# CrispLEDES 1.5.8 release

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
