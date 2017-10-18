### Deployment Instructions (Windows 10 Only)
1. [Clone](https://help.github.com/articles/cloning-a-repository/) this repository.
2. Download/Install [Unity](https://store.unity.com/) (Personal) and [Microsoft Visual Studio](https://www.visualstudio.com/downloads/) (Community).
3. In Unity, build project.
4. In Visual Studio:
    1. set "Solution Configurations" to "Release"
    2. set "Solution Platform" to "x86"
    3. set "Emulator" to "Remote Machine"
    4. set "Debug -> Project Properties -> Debug -> Remote Machine -> Find" to Microsoft HoloLens IP Address
5. Deploy to HoloLens: "Debug -> Start Without Debugging"
6. Open "Telemed3D" application through Microsoft HoloLens.