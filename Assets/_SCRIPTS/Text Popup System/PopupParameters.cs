namespace Management.UserInterface.FloatingTextPopupSystem
{
    public class PopupParameters
    {
        public float moveXSpeed = 0.2f;
        public float moveYSpeed = 2.5f;

        public float disappearTimerMax = 0.7f;
        public float disappearSpeed = 3f;

        public float normalTextSize = 0.3f;
        public float criticalTextSize = 0.35f;
        public float textStartingAlpha = 1f;

        public string normalTextColor = "FFF745";
        public string criticalTextColor = "FF4900";

        public float scaleIncreaseAmount = 1f;
        public float scaleDecreaseAmount = 1f;

        public int minSortingLayer = 0;


        public PopupParameters(){}
        
        public PopupParameters(float moveXSpeed = 0.2f, float moveYSpeed = 2.5f,
            float disappearTimerMax = 0.7f, float disappearSpeed = 3f,
            float normalTextSize = 0.3f, float criticalTextSize = 0.35f, float textStartingAlpha = 0.8f,
            string normalTextColor = "FFF745", string criticalTextColor = "FF4900",
            float scaleIncreaseAmount = 1f, float scaleDecreaseAmount = 1f,
            int minSortingLayer = 0)
        {
            this.moveXSpeed = moveXSpeed;
            this.moveYSpeed = moveYSpeed;
            this.disappearTimerMax = disappearTimerMax;
            this.disappearSpeed = disappearSpeed;
            this.normalTextSize = normalTextSize;
            this.criticalTextSize = criticalTextSize;
            this.textStartingAlpha = textStartingAlpha;
            this.normalTextColor = normalTextColor;
            this.criticalTextColor = criticalTextColor;
            this.scaleIncreaseAmount = scaleIncreaseAmount;
            this.scaleDecreaseAmount = scaleDecreaseAmount;
            this.minSortingLayer = minSortingLayer;
        }
    }
}