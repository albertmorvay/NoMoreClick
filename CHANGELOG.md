# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]
- https://github.com/albertmorvay/NoMoreClick/labels/enhancement

## 0.12.0 - 2022-01-09
### Added
- Whilst Caps Lock is enabled, a double left click will be performed when the mouse stops; this is instead of the default single left click.

## 0.11.1 - 2021-12-12
### Fixed
- Setup guardrails to initialize into a safe default state if appsettings.json is not present on startup or values are set too low by the user to an extent that is may cause unpredictable auto clicks.

## 0.11.0 - 2021-12-12
### Added
- Now starts normally and also shows up in the taskbar, providing visual feedback that the application has launched. When minimizing, a notification is shown informing the user that the application is still running and can be found in the taskbar notification area. 

## 0.10.0 - 2021-12-12
### Added
- Using the config file it is possible to specify two custom .wav (Waveform Audio File Format) file paths which will be played respectively to provide feedback when you have held down the right mouse button long enough to register intent to turn mouse click assistance on or off. Removing the properties, specifying an empty string as the value e.g. "", or specifying an invalid file path e.g. to a file that does not exist or failing to properly escape a backslash by doubling it up \\, will result in the built in sounds being played. 
### Changed
- Visual Studio Project during building now targets 64-bit platforms as I am exclusively using this on 64-bit systems. The TRACE compiler directive has been disabled as I do not use it.
- Updated assembly metadata with correct date and adding the repository GitHub URL to the description.

## 0.9.1 - 2021-12-05
### Changed
- Default values in configuration changed to better suite my personal preferences.

## 0.9.0 - 2021-12-05
### Added
- Appears in icon tray instead of taskbar, minimize overwritten for smoother transition to icon tray.
- Form location moved to just above icon tray.
- Minimal context menu added to icon tray icon allowing the application to be restored and closed.

## 0.8.0 - 2021-12-04
### Added
- Added configuration.json file to enable easier fine tuning of the application. 

## 0.7.0 - 2021-12-03
### Added
- Click same spot after moving mouse outside of 20 pixel dead zone around last click position without stopping to register a click. 

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
