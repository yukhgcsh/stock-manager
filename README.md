# 株式収支管理アプリケーション

日本国内の株式や投資信託の売買履歴や損益を管理するためアプリケーションです。  

![ダッシュボード][dashboard]

## 機能

* トータルでの損益などを表示するダッシュボード画面
  * 現時点では株式の情報のみを表示(投資信託未対応)
* 株式の取引履歴の管理
* 株式の配当金の管理
* 投資信託の取引履歴の管理
* 投資信託の分配金の管理

![株式一覧][stock-list]
![株式取引履歴][stock-transaction]

## 追加予定機能

* 投資信託の損益をダッシュボードに反映
* 含み益、含み損の反映

## 実行方法

> dockerの実行環境が必要です。

1. ビルドスクリプトの実行
   1. cd scripts/build
   2. powershell -ExecutionPolicy Unrestricted .\build.ps1
2. アプリケーションの起動
   1. cd dist
   2. docker-compose up -d

[dashboard]: doc/images/dashboard.png
[stock-list]: doc/images/stock-list.png
[stock-transaction]: doc/images/stock-transaction.png