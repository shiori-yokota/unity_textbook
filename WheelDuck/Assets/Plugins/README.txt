【IronPythonを用意する】
IronPythonのホームページ (http://ironpython.net/) からIronPython2.7を
ダウンロードする
　　# Download IronPythonをクリックしていけばmsi版がある
　　# zip版もあるがPythonモジュールが含まれていないためmsi版をダウンロードする
　　# msiは32bitなのでx64環境のWindowsでは(x86)がついたディレクトリに展開する
	
UnityのアセットにIronPythonを組み込む
IronPythonのPlatforms/Net35フォルダにある以下の6つのdllファイルを
Unityのアセットに登録（コピー）する
　　# IronPython.dll
　　# IronPython.Modules.dll
　　# Microsoft.Dynamic.dll
　　# Microsoft.Scripting.Core.dll
　　# Microsoft.Scripting.dll
　　# Microsoft.Scripting.Metadata.dll

Pythonの標準モジュールを使用する
IronPythonのLibフォルダをWheelDuck/Python/フォルダにコピーする

Unityのプロジェクト設定を変更する
組み込んだIronPythonが動作するようにプロジェクトの設定を変更する
　　# Edit -> Project Settings -> Player メニューから
　　　 PlayerSettings インスペクターを開きOptimization セクションにある
　　　 Api Compatibility Level を .NET 2.0に設定する
