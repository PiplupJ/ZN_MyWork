# このレポジトリはゲームの制作フォルダではありません。
プロジェクトの中で、私が担当したソースコードを紹介するためのミラーレポジトリです。

# ZN

## 1. 作品紹介

攻撃・回避・パリィを活用し、ボスの撃破を目指すアクションゲームです。
死亡するたびに強くなる仕組みと無敵の必殺技を取り入れ、繰り返し挑戦してクリアを目指せるようにしました。

- 開発環境：Unity 6000.0.23f1 / C#
- 制作期間：2025年9月17日〜2026年1月24日、2026年5月15日〜10月12日（改修）
- 制作人数：当初8人（デザイナー5人・プログラマー3人）、後半5人（デザイナー2人・プログラマー3人）
- 動作環境：Windows

## 2. 担当ソースコード

以下はこのレポジトリ内のパスです。

### 新規作成

以下のフォルダー内のスクリプトを作成しました。

- [`Script/Enemy/Base/`](Script/Enemy/Base/)
- [`Script/Enemy/Golem/`](Script/Enemy/Golem/)
- [`Script/Enemy/Zenith/`](Script/Enemy/Zenith/)
- [`Script/GameFlow/AttackHitBox/`](Script/GameFlow/AttackHitBox/)
- [`Script/GameFlow/Effect/`](Script/GameFlow/Effect/)
- [`Script/GameFlow/Sound/`](Script/GameFlow/Sound/)

以下のファイルも作成しました。

- `Script/GameFlow/BattleDefine.cs`
- `Script/GameFlow/DebugSceneSwitcher.cs`
- `Script/GameFlow/EnemyAttackHitBox.cs`
- `Script/GameFlow/EnemyEffectHitbox.cs`
- `Script/GameFlow/KnockbackTable.cs`
- `Script/GameFlow/ProjectileAttackHitBox.cs`
- `Script/GameFlow/ProjectileMoveData.cs`
- `Script/GameFlow/ProjectileShooter.cs`
- `Script/GameFlow/RocketMoveData.cs`
- `Script/GameFlow/VideoSceneLoader.cs`

### 引き継ぎ・改修を含む担当箇所

- [`Script/Player/`](Script/Player/)

プレイヤー制御は前担当者から引き継ぎました。このフォルダーには、前担当者のコードと、私が新規作成・改修したコードが混在しています。

以下のファイルは新規作成

- `Script/Player/カメラ/OccluderFader.cs` - CameraControllerから呼び出し
- `Script/Player/攻撃処理/ComboData.cs`
- `Script/Player/ステート/NadirCounterAttackState.cs`
- `Script/Player/ステート/NadirImpactState.cs`
- `Script/Player/ステート/NadirParryState.cs`
- `Script/Player/ダメージ処理/ParryData.cs`
- `Script/Player/ダメージ処理/PlayerDamageDelegate.cs`
- `Script/Player/ダメージ処理/PlayerHitSound.cs`
- `Script/Player/ダメージ処理/PlayerParry.cs`

以下のファイルを改修

- `Script/Player/ステート/NadirAttackState.cs` - 外部データを使った攻撃・先行入力・攻撃段階の状態遷移に対応
- `Script/Player/NadirBaseState.cs` - 移動の加減速・スティックの傾きによる歩き・走りの切り替え、パリィへの遷移、攻撃時のボスへの回転を追加
- `Script/Player/ステート/NadirMoveState.cs` - NadirBaseStateの加減速処理を使った移動に変更
- `Script/Player/NadirStateMachine.cs` - 加減速・パリィ・ノックバックに必要な変数・関数追加
- `Script/Player/ステート/NadirSuperAttackingState.cs` - 共通ヒットボックスに変更した必殺技弾に対応
- `Script/Player/PlayerAnimation.cs` - Animationの切り替えをトリガーからCrossFadeInFixedTimeに変更
- `Script/Player/ダメージ処理/PlayerHP.cs` - IDamageableの実装、パリィ判定・ノックバック・被弾サウンド・無敵状態の処理追加
- `Script/Player/攻撃処理/WeaponController.cs` - 共通ヒットボックスを継承した仕様に変更。武器エフェクト管理処理追加

## 3. 素材出典

### チームのデザイナーが制作

- 全キャラクター・ステージのモデル、UI用画像

### 外部素材

サウンド

- https://assetstore.unity.com/packages/audio/music/rock/metal-pack-22333
- https://assetstore.unity.com/packages/audio/music/rock/metal-drum-bass-music-pack-power-trip-387450
- https://assetstore.unity.com/packages/audio/music/divine-sacred-choral-collection-344472
- https://assetstore.unity.com/packages/audio/sound-fx/weapons/melee-weapons-sound-effects-pack-1-231154
- https://assetstore.unity.com/packages/audio/sound-fx/weapons/laser-shot-sci-fi-pack-v1-142033

Skybox

- https://assetstore.unity.com/packages/3d/environments/aaa-space-nebula-hdri-pack-11-ultra-hd-8k-skyboxes-exr-jpg-390748

エフェクト

- https://assetstore.unity.com/packages/vfx/particles/hit-effects-284955

一部のモーション

- https://www.mixamo.com/
