

set UNITY_PATH=D:\Unity\2018.4.36f1\Editor\Unity.exe
set MY_PATH=E:\kaspo\exam\lagunagames\LG-AssetBundle

start %UNITY_PATH% -quit -batchmode -logFile stdout.log -projectPath %MY_PATH% -executeMethod AssetBundleCreator.BuildBundles