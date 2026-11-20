# NoMoreClick
Prolonged use of a mouse is one of the common causes of Repetitive Strain Injury (RSI) [more on that here](https://www.nhs.uk/live-well/healthy-body/tips-to-prevent-rsi/); NoMoreClick tries to reduce the user's need for physical interaction with the mouse by performing a click automatically whenever the user stops moving the mouse.

## Motivation

The motivation behind the creation and maintenance of this project is the same as all my other projects, I enjoy tinkering in my spare time; I also wanted a bespoke auto-click solution in this case without any other bells and whistles.

## Usage

### How to download a pre-built copy

https://user-images.githubusercontent.com/5757588/145997378-bb2fcdaa-2790-4962-93b5-6a54f614ad46.mp4

### Introduction to core functionality

https://user-images.githubusercontent.com/5757588/145997419-940327ef-e385-47b3-8aab-cdb78179bdeb.mp4

With this software running:
- Just move the mouse cursor over what you want to click and let the software do the clicking.

- You can stop hovering your finger over the mouse button in a tense "ready to click" posture.

- Learn to increase the time your mousing hand gets to rest, by unlearning any unnecessary mousing habits you may have developed such as moving the mouse to point at what you are reading, as in time you may learn to only touch and move the mouse if you want to click.

## Installation

Run the self-contained Windows x64 build to use NoMoreClick without a separate runtime installation. A framework-dependent build requires the [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0).

NoMoreClick starts in the notification area. Open the tray menu to pause, resume, change settings, or exit. You can also hold the right mouse button still until the dot brightens, then release it to toggle assistance. This takes about one second by default. A right-button drag uses the ordinary drag gesture.

- Hold Ctrl while dwelling to pass Ctrl+click through to the foreground app.
- Hold Alt while dwelling to right-click. You can change or disable this modifier in Settings.
- Tap left Shift twice to make the next unmodified dwell click a double-click.
- After a click focuses a supported writable text field, assistance briefly pauses so you can start typing.
- Adaptive delay is optional. It starts at your configured dwell delay and learns from recent manual clicks, within 25% of that delay. Settings and learned timing apply to the current session.

## Development setup

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) on Windows. Use this command to build the cloned repository.

```powershell
dotnet build src/NoMoreClick.sln -c Release
```

Use this command to publish a self-contained Windows x64 executable.

```powershell
dotnet publish src/NoMoreClick/NoMoreClick.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=embedded
```

You can also open `src/NoMoreClick.sln` in an IDE that supports .NET 10.

### Build or publish with GitHub Actions

Open the **Windows Release** workflow in the repository’s Actions tab and select **Run workflow**. Run it on `master` to build a Windows x64 executable on GitHub’s standard Windows runner. The executable is available under **Artifacts** when the run completes.

To publish a release, first update the version in `src/NoMoreClick/NoMoreClick.csproj` and merge it into `master`. Select **Publish a GitHub Release for the version in the project** when starting the workflow. GitHub attaches the executable to the new version’s release.

## Contributing

1. Fork it (<https://github.com/albertmorvay/NoMoreClick/fork>)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Amend CHANGELOG.md following instructions within the file
4. Commit your changes (`git commit -am 'Add some fooBar'`)
5. Push to the branch (`git push origin feature/fooBar`)
6. Create a new Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE file](LICENSE) for details.

## Dependencies

NoMoreClick uses WinForms, Windows UI Automation, and the Windows input APIs. The application has no third-party NuGet packages. See [third-party asset attribution](LICENSE-THIRD-PARTY.md) for the bundled icon.
