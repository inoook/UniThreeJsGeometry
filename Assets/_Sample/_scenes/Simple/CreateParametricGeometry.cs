using UnityEngine;
using System.Collections;
using THREE;

public class CreateParametricGeometry : CreateBase {

    [SerializeField] int slices = 8;
    [SerializeField] int stacks = 32;
    
    public override void Create() {
        geo = new ParametricGeometry(paramFunc, slices, stacks);
	}

	// http://www56.atwiki.jp/threejs/pages/56.html
	Vector3 paramFunc(float u, float v){
		// uとvは0～1までの値をもらえる
		u *= 1;                //   uの定義域を設定。この例では0～1のまま。0～2πにしたいときなど適宜設定。
		v *= 1;                //   vの定義域を設定。
		// パラメトリック曲面のある点の
		float z = u * 130;       // Z座標は u * 130
		float x = v * 130;       // X座標は v * 130
		float y;                 // Y座標は
		if(u<0.5f && v<0.25f){   //   0≦u＜0.5 かつ 0≦v＜0.25のとき
			y = 0;               //     0
		}else if(0.5f<=u && u<=0.75f  &&  0.25f<=v && v<=0.5f){
			//   0.5≦u≦0.75 かつ 0.25≦v≦0.5のとき
			y = 26;              //     26
		}else{                 //   それ以外のとき
			y = 0;               //     0
		}                      // とする。
		return new Vector3(x,y,z);
	}
}
