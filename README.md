# UnityPackageManagerEditor
- [README.md](./Assets/com.akihiro.upmeditor/Documentation/README.md)

## Add Unity Package Manager
- Unity Editor のメニュー `Window/Package Manager` ウィンドウの左上 `+` ボタンを押下
- `Add Package from git URL ...` を選択し以下のURLを入力して `Add` でパッケージ追加

- https
    - `git+https://github.com/akihiro0105/UnityPackageManagerEditor.git?path=/Assets/com.akihiro.upmeditor/`

or

- ssh
    - `git+ssh://git@github.com/akihiro0105/UnityPackageManagerEditor.git?path=/Assets/com.akihiro.upmeditor/`

# 依存パッケージ
- Unity 2019.4.28f1 以下は `JSON .NET For Unity` を Unity プロジェクトにインポートしてください
    - [Unity Asset Store](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347)
    - [GitHub](https://github.com/jilleJr/Newtonsoft.Json-for-Unity)

# 動作確認バージョン
- Unity 2019.4.16f1
- Unity 2019.4.30f1
- Unity 2020.3.20f1
- Unity 2020.3.22f1
