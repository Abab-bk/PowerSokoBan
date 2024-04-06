extends Node

const APP_ID:String = "12935"

signal get_reward
signal reward_skip
signal rewrad_closed
signal reward_failed

var ad

var _plugin_name = "RealPocket"
var _android_plugin

func _ready():
    if Engine.has_singleton(_plugin_name):
        _android_plugin = Engine.get_singleton(_plugin_name)
        _android_plugin.initAd(APP_ID)
        _android_plugin.RewardGet.connect(func():
            get_reward.emit())
        _android_plugin.RewardSkip.connect(func():
            reward_skip.emit())
        _android_plugin.RewardFailed.connect(func():
            reward_failed.emit())
        _android_plugin.RewardClosed.connect(func():
            rewrad_closed.emit())
    else:
        print("Couldn't find plugin " + _plugin_name)


func show_reward_video_ad(_ad_id:String = "57794") -> void:
    if not _android_plugin:
        reward_failed.emit()
        return
    _android_plugin.ShowRewardVideoAd(_ad_id)
