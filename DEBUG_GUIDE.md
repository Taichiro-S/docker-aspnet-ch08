# VS Code Dev Container でデバッグ実行する手順

このプロジェクトを VS Code の Dev Container でデバッグ実行するための完全ガイドです。

## 前提条件

- Docker Desktop がインストール済みで起動している
- VS Code がインストール済み
- VS Code の拡張機能「Dev Containers」がインストール済み

## セットアップ手順

### 1. Dev Container を開く

1. VS Code でこのプロジェクトフォルダを開く
2. `Ctrl + Shift + P` でコマンドパレットを開く
3. 「Dev Containers: Reopen in Container」を選択
4. コンテナのビルドと起動を待つ（初回は数分かかります）

### 2. 依存関係の復元（初回のみ）

Dev Container が起動したら、ターミナルで以下を実行:

```bash
cd /workspace/src
dotnet restore
```

### 3. デバッグ実行

#### 方法1: F5キーでデバッグ開始

1. ブレークポイントを設定したい行番号の左側をクリック
2. `F5` キーを押す
3. 初回はビルドが走り、その後アプリが起動します

#### 方法2: Run and Debug パネルから

1. 左サイドバーの「Run and Debug」アイコンをクリック
2. 上部のドロップダウンから「.NET Core Launch (web)」を選択
3. 緑の再生ボタンをクリック

### 4. アプリケーションへのアクセス

デバッグが開始されると、以下のポートが転送されます:

- **http://localhost:5000** - .NET アプリケーション（直接アクセス）
- **http://localhost:8080** - Nginx 経由でのアクセス
- **http://localhost:5050** - pgAdmin（DB管理ツール）

### 5. 動作確認

1. ブラウザで http://localhost:5000 を開く
2. ヘルスチェック: http://localhost:5000/api/health
3. 静的ファイル: http://localhost:5000/index.html

## デバッグのヒント

### ブレークポイントの設定

- コントローラーのメソッドにブレークポイントを設定
- 例: `app/src/Controllers/AuthController.cs` の各エンドポイント

### 変数の確認

- デバッグ実行中は左サイドバーの「Variables」パネルで変数の値を確認可能
- Watch 式を追加して特定の式を監視できる

### デバッグコンソール

- デバッグ中に `Debug Console` タブで式を評価可能
- 例: 変数名を入力すると現在の値が表示される

### ホットリロード

通常の実行（デバッグなし）では `dotnet watch` が有効で、コードを変更すると自動で再ビルド・再起動します。

## よくある問題と解決策

### ポートが既に使用されている

```bash
# コンテナを完全に停止
docker-compose -f .devcontainer/docker-compose.yml down

# 再度 Dev Container を開く
```

### データベース接続エラー

```bash
# DB コンテナが起動しているか確認
docker ps | grep postgres

# DB の初期化スクリプトを確認
# db/init/01_init.sql が実行されているか確認
```

### デバッガーがアタッチできない

1. `launch.json` の `program` パスが正しいか確認
2. ビルドが成功しているか確認: `dotnet build app/src/Program.csproj`
3. VS Code を再読み込み: `Ctrl + Shift + P` → "Developer: Reload Window"

### 拡張機能が見つからない

Dev Container 内で必要な拡張機能が自動インストールされない場合:

1. `.devcontainer/devcontainer.json` を確認
2. コンテナを再ビルド: `Ctrl + Shift + P` → "Dev Containers: Rebuild Container"

## プロジェクト構造

```
docker-aspnet-ch08/
├── .devcontainer/
│   ├── devcontainer.json      # Dev Container 設定
│   └── docker-compose.yml     # マルチコンテナ構成
├── .vscode/
│   ├── launch.json            # デバッグ設定
│   └── tasks.json             # ビルドタスク
├── app/
│   ├── src/
│   │   ├── Program.cs         # エントリーポイント
│   │   ├── Program.csproj     # プロジェクトファイル
│   │   ├── Controllers/       # API コントローラー
│   │   ├── Models/            # データモデル
│   │   ├── DTOs/              # データ転送オブジェクト
│   │   ├── Services/          # ビジネスロジック
│   │   └── wwwroot/           # 静的ファイル
│   └── Dockerfile
├── db/
│   └── init/
│       └── 01_init.sql        # DB 初期化スクリプト
└── web/
    ├── Dockerfile
    └── nginx.conf             # リバースプロキシ設定
```

## 開発のワークフロー

1. **Dev Container を開く** - 環境が自動的にセットアップされる
2. **コードを編集** - IntelliSense や補完が効く
3. **デバッグ実行** - F5 でブレークポイントを使った調査
4. **ホットリロード** - `dotnet watch` で変更を即座に反映
5. **テスト** - ブラウザや Postman で API をテスト
6. **コミット** - Git で変更を記録

## 追加の便利なコマンド

### マイグレーション関連

```bash
# マイグレーションの追加
cd /workspace/src
dotnet ef migrations add InitialCreate

# データベース更新
dotnet ef database update

# マイグレーション履歴の確認
dotnet ef migrations list
```

### パッケージ管理

```bash
# パッケージの追加
dotnet add package PackageName

# パッケージの更新
dotnet restore
```

### ログの確認

```bash
# アプリケーションログ
docker-compose -f .devcontainer/docker-compose.yml logs app

# DB ログ
docker-compose -f .devcontainer/docker-compose.yml logs db
```

## Pragmatic Programmer のヒント

### 1. 早期フィードバック
デバッガーを活用して、コードが期待通り動くか即座に確認。推測ではなく、実際の値を見る。

### 2. DRY（Don't Repeat Yourself）
共通のビルドやデバッグ設定は `.vscode/` に集約。チーム全体で同じ環境を共有。

### 3. 自動化
Dev Container により、環境構築を自動化。「私のマシンでは動く」問題を解消。

### 4. デバッグは武器
print デバッグだけでなく、ブレークポイントやウォッチ式を駆使して効率的に問題を特定。

### 5. インフラをコード化
Docker Compose でインフラ構成を管理。誰でも同じ環境を再現可能に。
