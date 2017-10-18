# Deployment Instructions
### Telemed3D (Windows 10 Only)
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

### itSeez3D (iPad and Structure Sensor)
1. Download "itSeez3D" from the App Store.
2. Create profile online at https://itseez3d.com/ and choose payment plan (For now, we are using a non-commercially authorized account with username telemed3d@gmail.com.  Please contact the Telemed3D team for the password.).
3. Attach Structure Sensor as per the specific Structure Sensor's instructions.
4. Open itSeez3D application via iPad and login to account.

# Software Tutorial
### Telemed3D
1. Once deployed, open Telemed3D application via Microsoft HoloLens.
2. Tap "Open File" to add a 3D object on-screen.
3. Expand object by holding tap and dragging from corner (ball shape) of 'manipulator box.'
    1. Set gaze on manipulator ball before tapping and dragging.
    2. To increase intensity of scaling, slowly track hand as manipulator is moved to edge of scaling range.
4. Rotate object by holding tap, dragging, and releasing tap.
    1. To stop rotation as object moves, tap once on the object.
    2. To increase intensity of rotation, slowly track hand as manipulator is moved to edge of rotation range.
5. Tap "Reset" to return object to original size/rotation.

### itSeez3D
1. Once logged into account in itSeez3D, choose "New Scan +" in the top right corner.
2. Choose the proper scan setting:
    1. Bust: for a 3D scan of the patient's head.
    2. Fullbody: for a 3D scan of the patient's full body.
    3. Object: for a 3D scan of a patient's body part resting on a flat surface.
    4. Environment: for a 3D scan of a patient's body part in any location.
3. Once scan begins, slowly move around subject until part is fully engulfed in the clay-like imaging.  Assure that the subject remains still during this process.
4. Once scan is complete, tap scanned item in "MY MODELS" to view menu.
5. View "Local preview" to assure that the scan is complete.
6. If the "Local preview" appears complete, click "Cloud processing" and wait for computation to complete.
7. The 3D image may now be viewed in-app and downloaded online at https://itseez3d.com/
