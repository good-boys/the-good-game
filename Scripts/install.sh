#! /bin/sh

BASE_EDITOR_URL=https://netstorage.unity3d.com/unity
BASE_INSTALLER_URL=http://download.unity3d.com/download_unity
HASH=38bd7dec5000
VERSION=2018.2.11f1

download() {
  base_url=$1
  package=$2
  url="$base_url/$HASH/$package"

  echo "Downloading from $url: "
  curl -o `basename "$package"` "$url"
}

install() {
  package=$1
  download "$package"

  echo "Installing "`basename "$package"`
  sudo installer -dumplog -package `basename "$package"` -target /
}

# See $BASE_URL/$HASH/unity-$VERSION-$PLATFORM.ini for complete list
# of available packages, where PLATFORM is `osx` or `win`

// http://download.unity3d.com/download_unity/38bd7dec5000/MacEditorTargetInstaller/UnitySetup-Windows-Mono-Support-for-Editor-2018.2.11f1.pkg
install $BASE_EDITOR_URL "MacEditorInstaller/Unity-$VERSION.pkg"
install $BASE_INSTALLER_URL "MacEditorTargetInstaller/UnitySetup-Windows-Mono-Support-for-Editor-$VERSION.pkg"
install $BASE_INSTALLER_URL "MacEditorTargetInstaller/UnitySetup-Mac-Support-for-Editor-$VERSION.pkg"
install $BASE_INSTALLER_URL "MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-$VERSION.pkg"
