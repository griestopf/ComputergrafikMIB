using System;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    public class FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private float _camAngle = 0;
        private TransformComponent _cubeTransform;

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0f, 0, 0f, 1);

            // Create a scene with a cube
            // The three components: one XForm, one Shader and the Mesh
            _cubeTransform = new TransformComponent {Scale = new float3(1, 1, 1), Translation = new float3(0, 0, 0)};

            int l = 10;

            SceneNodeContainer[] cubes = new SceneNodeContainer[l];

            for(int i = 1; i <= l; i++){
                var tempCube = new SceneNodeContainer();
                tempCube.Components = new List<SceneComponentContainer>();
                tempCube.Components.Add(new TransformComponent {Scale = new float3(2, 2, 2), Translation = new float3((1 + 2*i) - (l), (1 + 2*i) - (l), (1 + 2*i) - (l))});
                tempCube.Components.Add(new ShaderEffectComponent{ Effect = SimpleMeshes.MakeShaderEffect(new float3 (200, 1, 1), new float3 (1, 1, 2),  4)});
                tempCube.Components.Add(SimpleMeshes.CreateCuboid(new float3(1, 1, 1)));
                cubes[i - 1] = tempCube;
            }

            /*
            var cubeNode2 = new SceneNodeContainer();
            cubeNode2.Components = new List<SceneComponentContainer>();
            cubeNode2.Components.Add(new TransformComponent {Scale = new float3(2, 2, 1), Translation = new float3(0, 0, 10)});
            cubeNode2.Components.Add(new ShaderEffectComponent{ Effect = SimpleMeshes.MakeShaderEffect(new float3 (0, 0, 1), new float3 (1, 1, 1),  4)});
            cubeNode2.Components.Add(SimpleMeshes.CreateCuboid(new float3(10, 10, 10)));
            */

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children = new List<SceneNodeContainer>();
            foreach(SceneNodeContainer c in cubes){
                _scene.Children.Add(c);
            }

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Animate the camera angle
            _camAngle = _camAngle + 90.0f * M.Pi/180.0f * DeltaTime;
            //Diagnostics.Log(_camAngle.ToString());

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle) + float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationX(_camAngle);

            // Animate the cube
            _cubeTransform.Translation = new float3(0, 5 * M.Sin(3 * TimeSinceStart), 0);

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }

       public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);
            
            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        
    }
}