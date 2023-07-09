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

1. Install [.NET Framework 4.8 Runtime](https://dotnet.microsoft.com/download/dotnet-framework/net48) or higher.
2. // TODO: for v1.00

## Development setup

Windows:
1. Install your favorite Integrated Development Environment (IDE) for C# e.g. [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/) or [Jetbrains Rider](https://www.jetbrains.com/rider/).

2. With an IDE installed, ensure that you have the [.NET Framework 4.8 Developer Pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48) installed.

3. If you haven't already install [Git](https://git-scm.com/downloads) and/or your preferred Git client e.g. [Github Desktop](https://desktop.github.com/) to gain access to the code.

4. Fork this repository and clone the repository.

> After cloning the repository you will have the `master` branch checked out, as the main branch it contains a stable "production-ready" state of the source code. As this is not likely the most recent version of the code, you will want to checkout a branch; check the Projects tab for Issues *In Progress*.

5. Using your IDE, open the project's `.sln` file. This will open the solution and projects within your IDE. Once open in the IDE, you are ready to go.


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

- [globalmousekeyhook](https://github.com/gmamaladze/globalmousekeyhook) licensed under the [MIT License](https://github.com/gmamaladze/globalmousekeyhook/blob/vNext/LICENSE.txt)

*The above is not an exhaustive list of dependencies, as these will by necessity also introduce their own dependencies to the solutions, for details please check the respective packages.*
