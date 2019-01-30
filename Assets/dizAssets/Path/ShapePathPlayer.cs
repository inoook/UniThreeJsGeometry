using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ShapePathPlayer : MonoBehaviour {
	
	public PointList pointList;
	
	private Vector3[] path;
	public ShapePath sp;// debug no tame public ni shiteiru
	
//	private float elapsedTime = 0;
	public float speed = 0.1f;
	public Transform target;
	
	void  Awake (){
		Debug.LogWarning("------------ ShapePathPlayer ------------");
		path = pointList.GetPathPoints();
		sp = pointList.GetShapePath();
	}
	
//	private bool isPlay = true;
	
	public float damping = 6.0f;
	
	public bool smooth = false;
	public Vector3 targetPos;
	
	void  Update (){
		
		if(Application.isEditor){
			path = pointList.GetPathPoints();
			sp = pointList.GetShapePath();
			
			DrawLinePoints(sp);
		}
		
		if(Application.isPlaying){
			if(isPlayAnim && smooth){
				target.transform.position = Vector3.Lerp(target.transform.position, targetPos, Time.deltaTime * damping);
				
				float dist = Vector3.Distance(target.transform.position, targetPos);
				//if(dist < 0.01f){
				if(dist < 0.1f && progress == 1.0f){
					isPlayAnim = false;
				}
			}
		}
	}
	
	public int sliceNum = 200;
	
	private void DrawLinePoints(ShapePath sp)
    {
		/*
		float split = sp.length / (sliceNum-1);
		
		LineRenderer lineRenderer = this.gameObject.GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(sliceNum);
		for(int i = 0; i < sliceNum; i++){
			Vector3 sPos = sp.getPointInfoVec3(split * i);
			lineRenderer.SetPosition(i, sPos);
		}
		*/
    }
	
	public Color pathColor = Color.green;
	
	void  OnDrawGizmos (){
		if(sp == null){ return; }
		
		for(int n = 0; n < path.Length-2; n++)
		{
			Vector3 pos = Vector3.Lerp( path[n], path[n+1], 0.5f );
			
			Color c;
			c = Color.green;
			c.a = 0.5f;
			Gizmos.color = c;
			Gizmos.DrawSphere(path[n], 0.25f);
			
			c = Color.red;
			c.a = 0.5f;
			Gizmos.color = c;
			Gizmos.DrawSphere(pos, 0.5f);
			/*
			c = Color.yellow;
			c.a = 0.25f;
			Gizmos.color = c;
			Gizmos.DrawLine(pos, path[n]);
			Gizmos.DrawLine(pos, path[n+1]);
			*/
		}
		
//		Vector3 posC = Vector3.Lerp( path[path.Length-2], path[path.Length-1], 0.5f);
//		Vector3 posEnd = path[path.Length-1];
		
		//
		float splitPer = 1.0f / (sliceNum-1);
		
		
		Gizmos.color = pathColor;
		for(int i = 0; i < sliceNum; i++){
			Vector3 sPos = sp.getPointInfoVec3Percent(splitPer * i);
			Vector3 ePos = sp.getPointInfoVec3Percent(splitPer * (i+1));
			Gizmos.DrawLine(sPos, ePos);
		}
		
	}
	
	public bool isUsePoint = false;
	
	public float time = 15.0f;
	public bool isPlayAnim = false;
	
	public Vector3 startTargetPos;
	/*
	public void PlayPathAnime(System.Action<AbstractGoTween> onComplete = null)
	{
		if(smooth){
			targetPos = target.position;
		}
		
		isPlayAnim = true;
		
		
		if(!isUsePoint){
			sp = pointList.GetShapePath();
			
			pathProgressValue = 0.0f;
			
			GoTween tw = new GoTween(this, time, new GoTweenConfig().floatProp("pathProgressValue", 1.0f).setEaseType(GoEaseType.SineOut));
			tw.setOnCompleteHandler(onComplete);
			Go.addTween(tw);
		}else{
			path = pointList.GetPathPoints();
			startTargetPos = target.position;
			
			pointProgressValue = 0.0f;
			
			GoTween twPoint = new GoTween(this, time, new GoTweenConfig().floatProp("pointProgressValue", 1.0f).setEaseType(GoEaseType.SineOut));
			twPoint.setOnCompleteHandler(onComplete);
			Go.addTween(twPoint);
		}
		
	}
	*/
	public bool IsPlay()
	{
		return isPlayAnim;
	}

	public Transform lookAtUp;
	public float progress = 0.0f;
	
	public Transform lookAtTarget;
	
	//public bool isDebug = false;
	public Vector3 debug_posV;
	
	public float pathProgressValue
	{
		get{ return progress; }
		set{
			progress = value;
			
			Vector3 posV = sp.getPointInfoVec3Percent( progress );
			debug_posV = posV;
			
			//Vector3 lookPos = sp.getPointInfoVec3Percent( progress );
			
			if(!smooth){
				target.transform.position = posV;
//				Vector3 lookAtUpPos = lookAtUp.position - posV;
				target.transform.LookAt(lookAtTarget);
			}else{
				targetPos = posV;
			}
		}
	}
	
	public float pointProgressValue
	{
		get{ return progress; }
		set{
			progress = value;
			
			//Vector3 posV = (path[path.Length-1] - path[0])*progress + path[0];
			Vector3 posV = (path[path.Length-1] - startTargetPos)*progress + startTargetPos;
			
			// smooth
			targetPos = posV;
		}
	}
	
	public Vector3 GetPathPosById(int id)
	{
		return path[id];
	}
		

}
