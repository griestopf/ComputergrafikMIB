using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Engine.Core.Effects;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "ColorAndInput", Description = "Yet another FUSEE App.")]
    public class ColorAndInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private Transform _cubeTransform;
        private DefaultSurfaceEffect _cubeEffect;

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to "greenery" ;-) (https://store.pantone.com/de/de/color-of-the-year-2017/).
            RC.ClearColor = new float4(136f/255f, 176f/255f, 75f/255f, 1);

            // Create a scene with a cube
            // The three components: one Transform, one ShaderEffect (blue material) and the Mesh
            _cubeTransform = new Transform {Translation = new float3(0, 0, 0)};
            _cubeEffect = MakeEffect.FromDiffuseSpecular((float4)ColorUint.Blue, float4.Zero);
            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            // Assemble the cube node containing the three components
            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(_cubeEffect);
            cubeNode.Components.Add(cubeMesh);

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cubeNode);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);

            _camAngle = 0;
        }

        float _camAngle;

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Diagnostics.Debug(Input.Keyboard.LeftRightAxis);
            // Diagnostics.Debug(Input.Mouse.Velocity);

            _camAngle = _camAngle + 6.28f * Time.DeltaTime * Input.Keyboard.LeftRightAxis;
            _cubeEffect.SurfaceInput.Albedo = new float4(0, 0.5f + 0.5f * M.Sin(Time.TimeSinceStart), 0, 1);

            RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);
            _cubeTransform.Translation = new float3(0, 10 * M.Sin(Time.TimeSinceStart), 0);


            _sceneRenderer.Render(RC);

           // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

        public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45� Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        

    }
}
