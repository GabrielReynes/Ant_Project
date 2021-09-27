using UnityEngine;

namespace Additions
{
	public class MouseManager
	{
		private readonly int m_screenWidth;
		private readonly int m_screenHeight;

		public MouseManager(int _screenWidth, int _screenHeight)
		{
			m_screenWidth = _screenWidth;
			m_screenHeight = _screenHeight;
		}

		public void Update(ComputeShader _computeShader)
		{
			bool mouseDown = Input.GetMouseButton(0);
			_computeShader.SetBool("mouse_effect_activated", mouseDown);

			if (!mouseDown) return;
			
			_computeShader.SetFloat("mouse_pos_x", Input.mousePosition.x / Screen.width * m_screenWidth);
			_computeShader.SetFloat("mouse_pos_y", Input.mousePosition.y / Screen.height * m_screenHeight);
		}
	}
}