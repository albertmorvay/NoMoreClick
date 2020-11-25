# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]
- https://github.com/albertmorvay/NoMoreClick/labels/enhancement

## 0.6.0 - 2020-11-25
### Added
- No Accidental Click after Physical User Left Click

## 0.5.0 - 2020-11-23
### Added
- No Click After Mouse Wheel Scroll preventing accidental click.

## 0.4.0 - 2020-11-23
### Added
- The toggeling of mouse click assistance plays an English audio notification providing user feedback on whether the functionality is on or off.
### Changed
- mouseClickDelayInMilliseconds reduced from 360 to 280 to trigger a mouse click faster creating a more responsive experience.
- The interval of the timer was decreased from 20 to 10 to improve sensitivity.

## 0.3.0 - 2020-11-10
### Added
- Right-click mouse for 1 second to toggle mouse click assistance on or off.
- No click immediately after typing to prevent accidental click if mouse moved whilst typing.
- No click whilst mouse being dragged e.g. whilst text is being selected.
- Click delay after a right click is 1080 milliseconds versus 360 to add tolerance during context menu interactions.

## 0.1.0 - 2020-08-30
### Added
- Auto left-click mouse on user idle
- Application won't start if mouse cursor snap-to-default is enabled