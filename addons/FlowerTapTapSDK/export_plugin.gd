@tool
extends EditorPlugin

var export_plugin : AndroidExportPlugin

func _enter_tree():
    export_plugin = AndroidExportPlugin.new()
    add_export_plugin(export_plugin)
    add_autoload_singleton("Tap", "Tap.gd")


func _exit_tree():
    remove_export_plugin(export_plugin)
    export_plugin = null
    remove_autoload_singleton("Tap")


class AndroidExportPlugin extends EditorExportPlugin:
    var _plugin_name = "FlowerTapSDK"

    func _supports_platform(platform):
        if platform is EditorExportPlatformAndroid:
            return true
        return false

    func _get_android_libraries(platform, debug):
        if debug:
            return PackedStringArray([
            "FlowerTapTapSDK/bin/debug/RealTapSDK-debug.aar",
            "FlowerTapTapSDK/bin/libs/AntiAddiction_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/AntiAddictionUI_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/TapBootstrap_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/TapCommon_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/TapLogin_3.24.0.aar",
            ])
        else:
            return PackedStringArray([
            "FlowerTapTapSDK/bin/release/RealTapSDK-release.aar",
            "FlowerTapTapSDK/bin/libs/AntiAddiction_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/AntiAddictionUI_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/TapBootstrap_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/TapCommon_3.24.0.aar",
            "FlowerTapTapSDK/bin/libs/TapLogin_3.24.0.aar",
            ])


    func _get_android_dependencies(platform: EditorExportPlatform, debug: bool) -> PackedStringArray:
        return PackedStringArray([
            "cn.leancloud:storage-android:8.2.19",
            "io.reactivex.rxjava2:rxandroid:2.1.1",
            "cn.leancloud:realtime-android:8.2.19",
            "io.reactivex.rxjava2:rxandroid:2.1.1",
            
        ])

    func _get_android_manifest_element_contents(platform: EditorExportPlatform, debug: bool) -> String:
        return """
        <uses-permission android:name="android.permission.INTERNET"></uses-permission>
        """

    func _get_name():
        return _plugin_name
