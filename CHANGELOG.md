# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [3.0.2.10] - 2026-09-06

## [3.0.1.9] - 2026-03-11

### Fixed

- Code Coverage in CI (#91)

## [3.0.0.8] - 2026-02-24

### Changed

- From xunit v2 to v3 (#88)

## [2.0.1.7] - 2026-02-14

## [2.0.0.6] - 2026-02-08

### Changed

- When failing to match a log event the error message is more descriptive (#64)

## [1.1.2.7] - 2026-02-08

### Fixed

- Issue where IsEnabled was returning the opposite of what is should be (#67) 

## [1.1.1.5] - 2026-02-08

### Fixed

- Git Tagging workflow that has been broken by GitHub security updates (#47)

## [1.1.0.4] - 2025-12-17

### Added

- NuGet badge to README showing latest package version (#28)
- Support for controlling LogLevel for tests (#13)

## [1.0.1.3] - 2025-12-06

### Added

- NuGet metadata to csproj (#22)

### Changed

- Updated permissions on completed issue workflow to include `issues: write` and `pull-requests: read` (#17)
- Missing permissions in tag release workflow (#20)

### Fixed

- Permissions for tagging releases (#24)

## [1.0.0.2] - 2025-12-06

### Added

- GitHub Label Management workflow (#1)
- Project structure and initial files (#3)
- CI/CD pipeline setup (#8)
- Add initial implementation (#7)
- Comprehensive README documentation with installation and usage instructions (#11)

### Fixed

- Bug in release workflow permissions (#14)

[unreleased]: https://github.com/baynezy/StubLogger/compare/3.0.2.10...HEAD
[3.0.2.10]: https://github.com/baynezy/StubLogger/compare/3.0.1.9...3.0.2.10
[3.0.1.9]: https://github.com/baynezy/StubLogger/compare/3.0.0.8...3.0.1.9
[3.0.0.8]: https://github.com/baynezy/StubLogger/compare/2.0.1.7...3.0.0.8
[2.0.1.7]: https://github.com/baynezy/StubLogger/compare/2.0.0.6...2.0.1.7
[2.0.0.6]: https://github.com/baynezy/StubLogger/compare/1.1.2.7...2.0.0.6
[1.1.2.7]: https://github.com/baynezy/StubLogger/compare/1.1.1.5...1.1.2.7
[1.1.1.5]: https://github.com/baynezy/StubLogger/compare/1.1.0.4...1.1.1.5
[1.1.0.4]: https://github.com/baynezy/StubLogger/compare/1.0.1.3...1.1.0.4
[1.0.1.3]: https://github.com/baynezy/StubLogger/compare/1.0.0.2...1.0.1.3
[1.0.0.2]: https://github.com/baynezy/StubLogger/compare/4f6b6f73e69cd2384e2349cfed5924038070f236...1.0.0.2
