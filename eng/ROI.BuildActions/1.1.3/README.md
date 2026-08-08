# Vendored ROI.BuildActions 1.1.3 macOS lifecycle

This directory contains the public-safe macOS distribution-package lifecycle used by CrispLEDES. It was derived from the private `ROI.BuildActions` 1.1.3 package released as `build-actions-v1.1.3` from source commit `f30d989`.

Included behavior:

- Developer ID application and installer signing
- versioned distribution-package creation
- Apple notarization and stapling
- stapled-ticket, installer-signature, and Gatekeeper validation

Excluded behavior:

- 1Password and secret injection
- private NuGet feed configuration
- organization-specific application configuration
- build tasks not required by CrispLEDES

The public sample `src/configuration-file.txt` remains the only configuration bundled by this repository.
