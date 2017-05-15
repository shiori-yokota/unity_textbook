【IronPythonのインストール】
・IronPythonのホームページ (http://ironpython.net/) からIronPython2.7をダウンロードする
　　# Download IronPythonをクリックしていけばmsi版がある
　　# zip版もあるがPythonモジュールが含まれていないためmsi版をダウンロードする
　　# msiは32bitなのでx64環境のWindowsでは(x86)がついたディレクトリに展開する

・WheelDuck/Assets/Pluginsに移動する
　　UnityのアセットにIronPythonを組み込む
　　IronPythonのPlatforms/Net35フォルダにある以下の6つのdllファイルを
　　Unityのアセットに登録（コピー）する
　　　　- IronPython.dll
　　　　- IronPython.Modules.dll
　　　　- Microsoft.Dynamic.dll
　　　　- Microsoft.Scripting.Core.dll
　　　　- Microsoft.Scripting.dll
　　　　- Microsoft.Scripting.Metadata.dll

・WheelDuck/Python/Libに移動する
　　Pythonの標準モジュールを使用するため、
　　IronPythonのLibフォルダ内の全てをコピーする

・Unityのプロジェクト設定を変更する
　　組み込んだIronPythonが動作するようにプロジェクトの設定を変更する
　　　　# Edit -> Project Settings -> Player メニューから
　　　　# PlayerSettings インスペクターを開きOptimization セクションにある
　　　　# Api Compatibility Level を .NET 2.0 に設定する


【Directory & File configuration】
WheelDuck/
   |
   |
   |---- Assets/
   |          |
   |          + Plugins		IronPythonを組み込む
   |          + Resources	アセットのインスタンスを作成してゲームプレイで使用
   |                 |
   |                 + Prefabs/		環境のprefab
   |                 + Textures/	ゴールに到達した際のエフェクトのテクスチャ
   |                 + Treasures/	お宝の保存先
   |                 + GoodStamp	ゴールに到達した際のエフェクト
   |
   |          + Scenes		シーンの保存先
   |          + ScreenShot	画像の保存先
   |          + Scripts/	 	各章に必要なスクリプト(cs)の保存先
   |                 |
   |                 |---- Chapter~~/
   |
   |---- ProjectSettings/
   |          |
   |          |---- ~~.asset
   |
   |---- Python/
               |
               + Chapter~~/	各章のpythonコード保存先
               + Lib/		Pythonの標準モジュールを使用
