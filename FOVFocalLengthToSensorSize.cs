
///視野角を固定したままインスペクター上で焦点距離を自由に設定できるカメラ用コンポーネントです。

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

///アタッチしたコンポーネントにはUnity.Cameraコンポーネントが必須です。
[RequireComponent(typeof(Camera))]
public class FOVFocalLengthToSensorSize : MonoBehaviour
{
    [Tooltip("垂直方向の視野角(°)")]
    public float FOV;
    [Tooltip("焦点距離(mm)")]
    public float focalLength;
    [Tooltip("アスペクト比")]
    public float aspectRaio;
    private Camera cameraComponent;

    private void Reset()
    {
        cameraComponent = GetComponent<Camera>();
        FOV = cameraComponent.fieldOfView;
        focalLength = cameraComponent.focalLength;
        aspectRaio = cameraComponent.aspect;
    }

    private void OnValidate()
    {

        ///物理カメラプロパティをオンにしてください
        if (cameraComponent.usePhysicalProperties == false)
        {
            Debug.LogError("Error: Set Camera.usePhysicalProperties to True, CameraコンポーネントのPhysical CameraをOnにしてください");
        };

        cameraComponent.fieldOfView = FOV;
        cameraComponent.focalLength = focalLength;
        ///デフォルトで垂直方向の視野角を基準にセンサーサイズを計算します。
        cameraComponent.sensorSize = calculateSensorSize(focalLength, FOV, aspectRaio);
    }

    private Vector2 calculateSensorSize(float focalLength, float FOV, float aspectRatio)
    {
        var verticalSensorSize = focalLength * Mathf.Tan(FOV * Mathf.Deg2Rad / 2) * 2;
        return new Vector2(verticalSensorSize * aspectRatio, verticalSensorSize);
    }
}
