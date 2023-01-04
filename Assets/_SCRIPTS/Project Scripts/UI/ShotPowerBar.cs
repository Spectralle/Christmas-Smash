using System;
using UnityEngine;
using Shapes;
using UnityEngine.Rendering;

namespace CasualGame
{
    public class ShotPowerBar : ImmediateModeShapeDrawer
    {
        [Header( "Gameplay" )]
        [SerializeField] float chargeSpeed = 1;
		[SerializeField] float chargeDecaySpeed = 1;
		[NonSerialized] public bool isCharging = false;
		float charge;

		[Header( "Style" )]
		public Color tickColor = Color.white;
		public Gradient chargeFillGradient;
		[Range( 0, 0.2f )] public float powerBarThickness = 0.1f;
		[Range( 0, 0.05f )] public float powerBarOutlineThickness = 0.05f;
		[Range( 0, 0.2f )] public float ammoBarRadius = 0.1225f;
		[Range( 0f, ShapesMath.TAU / 2 )] public float powerBarAngularSpanRad;
		[Range( 0, 0.1f )] public float tickSizeSmol = 0.1f;
		[Range( 0, 0.1f )] public float tickSizeLorge = 0.1f;
		[Range( 0, 0.05f )] public float tickTickness;
		[Range( 0, 0.5f )] public float fontSize = 0.1f;
		[Range( 0, 0.5f )] public float fontSizeLorge = 0.1f;
		[Range( 0, 0.1f )] public float percentLabelOffset = 0.1f;
		[Range( 0, 0.4f )] public float fontGrowRangePrev = 0.1f;
		[Range( 0, 0.4f )] public float fontGrowRangeNext = 0.1f;


		[Header( "Animation" )]
		[Range( 0f, 0.3f )] public float fireSidebarRadiusPunchAmount = 0.3f;
		public AnimationCurve chargeFillCurve;
		public AnimationCurve animChargeShakeMagnitude = AnimationCurve.Linear( 0, 0, 1, 1 );
		[Range( 0, 0.05f )] public float chargeShakeMagnitude = 0.1f;
		public float chargeShakeSpeed = 1;

		private Camera _cam;
		

		private void Start() => _cam = Camera.main;

		void Update()
		{
			if (!Application.isPlaying)
				return;
			UpdateCharge();
		}
		
		public override void DrawShapes( Camera cam )
		{
			if(cam != _cam) // only draw in the player camera
				return;

			using (Draw.Command(cam))
			{
				Draw.ZTest = CompareFunction.Always; // to make sure it draws on top of everything like a HUD
				Draw.Matrix = transform.localToWorldMatrix; // draw it in the space of crosshairTransform
				Draw.BlendMode = ShapesBlendMode.Transparent;
				Draw.LineGeometry = LineGeometry.Flat2D;
				float radiusPunched = ammoBarRadius + fireSidebarRadiusPunchAmount;
				DrawBar(radiusPunched);
			}
		}

		public void UpdateCharge() {
			if( isCharging )
				charge += chargeSpeed * Time.deltaTime;
			else
				charge -= chargeDecaySpeed * Time.deltaTime;
			charge = Mathf.Clamp01( charge );
		}

		public void DrawBar(float barRadius)
		{
			// get some data
			float angRadMin = -powerBarAngularSpanRad / 2;
			float angRadMax = powerBarAngularSpanRad / 2;
			float angRadMinLeft = angRadMin + ShapesMath.TAU / 2;
			float angRadMaxLeft = angRadMax + ShapesMath.TAU / 2;
			float outerRadius = barRadius + powerBarThickness / 2;

			float chargeAnim = chargeFillCurve.Evaluate(charge);

			// charge bar shake:
			float chargeMag = animChargeShakeMagnitude.Evaluate(chargeAnim) * chargeShakeMagnitude;
			//Vector2 origin = fpsController.GetShake(chargeShakeSpeed, chargeMag); // do shake here
			float chargeAngRad = Mathf.Lerp(angRadMaxLeft, angRadMinLeft, chargeAnim);
			Color chargeColor = chargeFillGradient.Evaluate(chargeAnim);
			//Draw.Arc(origin, fpsController.ammoBarRadius, powerBarThickness, angRadMaxLeft, chargeAngRad, chargeColor);
			Vector2 thisPos = transform.position;
			Draw.Arc(thisPos, ammoBarRadius, powerBarThickness, angRadMaxLeft, chargeAngRad, chargeColor);

			Vector2 movingLeftPos = thisPos + ShapesMath.AngToDir(chargeAngRad) * barRadius;
			Vector2 bottomLeftPos = thisPos + ShapesMath.AngToDir(angRadMaxLeft) * barRadius;

			// bottom fill
			Draw.Disc(bottomLeftPos, powerBarThickness / 2f, chargeColor);

			// ticks
			const int tickCount = 7;

			Draw.LineEndCaps = LineEndCap.None;
			for (int i = 0; i < tickCount; i++)
			{
				float t = i / (tickCount - 1f);
				float angRad = Mathf.Lerp(angRadMaxLeft, angRadMinLeft, t);
				Vector2 dir = ShapesMath.AngToDir(angRad);
				//Vector2 a = origin + dir * outerRadius;
				Vector2 a = thisPos + dir * outerRadius;
				bool lorge = i % 3 == 0;
				Vector2 b = a + dir * (lorge ? tickSizeLorge : tickSizeSmol);
				Draw.Line(a, b, tickTickness, tickColor);

				// scale based on distance to real value
				float chargeDelta = t - chargeAnim;
				float growRange = chargeDelta < 0 ? fontGrowRangePrev : fontGrowRangeNext;
				float tFontScale = 1f - ShapesMath.SmoothCos01(Mathf.Clamp01(Mathf.Abs(chargeDelta) / growRange));
				float fontScale = ShapesMath.Eerp(fontSize, fontSizeLorge, tFontScale);
				Draw.FontSize = fontScale;
				Vector2 labelPos = a + dir * percentLabelOffset;
				string pct = Mathf.RoundToInt(t * 100) + "%";
				Quaternion rotation = Quaternion.Euler(0, 0, (angRad + ShapesMath.TAU / 2) * Mathf.Rad2Deg);
				Draw.Text(labelPos, rotation, pct, TextAlign.Right);
			}

			// moving dot
			Draw.Disc(movingLeftPos, powerBarThickness / 2f + powerBarOutlineThickness / 2f);
			Draw.Disc(movingLeftPos, powerBarThickness / 2f - powerBarOutlineThickness / 2f, chargeColor);

			//FpsController.DrawRoundedArcOutline(origin, barRadius, powerBarThickness, powerBarOutlineThickness, angRadMinLeft, angRadMaxLeft);
			FpsController.DrawRoundedArcOutline(thisPos, barRadius, powerBarThickness, powerBarOutlineThickness, angRadMinLeft, angRadMaxLeft);

			Draw.LineEndCaps = LineEndCap.Round;

			// glow
			Draw.BlendMode = ShapesBlendMode.Additive;
			Draw.Disc(movingLeftPos, powerBarThickness * 2, DiscColors.Radial(chargeColor, Color.clear));
			Draw.BlendMode = ShapesBlendMode.Transparent;
		}
    }
}