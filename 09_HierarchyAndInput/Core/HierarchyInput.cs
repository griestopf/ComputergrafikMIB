using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class HierarchyInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;
        private TransformComponent _baseTransform;
        private TransformComponent _bodyTransform;
        private TransformComponent _upperArmTransform;
        private TransformComponent _foreArmTransform;
        private TransformComponent _greifArmTransformleft;
        private TransformComponent _greifArmTransformright;
        private Boolean close = false;
        private Boolean open = false;
       

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 6, 0)
            };

            _upperArmTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(2, 4, 0)
            };
            _foreArmTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-2, 4, 0)
            };
            _greifArmTransformleft = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-1f, 5, 0)
            };
            _greifArmTransformright = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(1, 5, 0)
            };



            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    //greyBASE
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // MATERIAL COMPONENT
                            new MaterialComponent
                            {
                                Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                                Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                            },

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        },


                        Children = new List<SceneNodeContainer>
                        {
                        
                                                                              
                            //redARM
                            new SceneNodeContainer
                            {
                                Components = new List<SceneComponentContainer>
                                {
                                    // TRANSFROM COMPONENT
                                    _bodyTransform,

                               

                                    // MATERIAL COMPONENT
                                    new MaterialComponent
                                    {
                                        Diffuse = new MatChannelContainer { Color = new float3(1, 0, 0) },
                                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                                    },

                                    // MESH COMPONENT
                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                },

                                Children = new List<SceneNodeContainer>
                                {
                                    //Gelenk
                                    new SceneNodeContainer
                                    {
                                        Components= new List<SceneComponentContainer>
                                        {

                                            _upperArmTransform,

                                        },

                                        Children = new List<SceneNodeContainer>
                                        {
                                            //greenArm
                                            new SceneNodeContainer
                                            {

                                                Components = new List<SceneComponentContainer>
                                                {
                                                    // TRANSFROM COMPONENT
                                                    new TransformComponent
                                                    {
                                                        Rotation = new float3(0,0,0),
                                                        Scale = new float3 (1,1,1),
                                                        Translation = new float3(0,4,0)
                                                    },

                                                    // MATERIAL COMPONENT
                                                    new MaterialComponent
                                                    {
                                                        Diffuse = new MatChannelContainer { Color = new float3(0, 1, 0) },
                                                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                                                    },

                                                    // MESH COMPONENT
                                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                                },
                                                Children = new List<SceneNodeContainer>
                                                {
                                                    //Gelenk
                                                    new SceneNodeContainer
                                                    { 
                                                        Components = new List<SceneComponentContainer>
                                                        {
                                                            _foreArmTransform
                                                        },

                                                        Children = new List<SceneNodeContainer>
                                                        { 
                                                            //blueArm
                                                            new SceneNodeContainer
                                                            {
                                                                Components = new List<SceneComponentContainer>
                                                                {
                                                                    // TRANSFROM COMPONENT
                                                                    new TransformComponent
                                                                    {
                                                                        Rotation = new float3(0,0,0),
                                                                        Scale = new float3(1,1,1),
                                                                        Translation = new float3(0,4,0)
                                                                    },

                                                                    // MATERIAL COMPONENT
                                                                    new MaterialComponent
                                                                    {
                                                                        Diffuse = new MatChannelContainer { Color = new float3(0, 0, 1) },
                                                                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                                                                    },

                                                                    // MESH COMPONENT
                                                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))

                                                                },

                                                                Children = new List<SceneNodeContainer>
                                                                {
                                                                    //greifarmLager
                                                                    new SceneNodeContainer
                                                                    {
                                                                        Components = new List<SceneComponentContainer>
                                                                        {
                                                                            _greifArmTransformleft                                                                          
                                                                        },
                                                                        
                                                                        Children = new List<SceneNodeContainer>
                                                                        {
                                                                            new SceneNodeContainer
                                                                            {
                                                                                Components = new List<SceneComponentContainer>
                                                                                {
                                                                                    new TransformComponent
                                                                                    {
                                                                                        Rotation = new float3(0,0,0),
                                                                                        Scale = new float3(1,1,1),
                                                                                        Translation = new float3(0.25f,2,0)                                                                                        
                                                                                    },
                                                                                    // MATERIAL COMPONENT
                                                                                    new MaterialComponent
                                                                                    {
                                                                                        Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                                                                                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                                                                                    },

                                                                                    // MESH COMPONENT
                                                                                    SimpleMeshes.CreateCuboid(new float3(0.5f, 4, 0.5f))
                                                                                }
                                                                            },

                                                                        }
                                                                       
                                                                    },
                                                                    new SceneNodeContainer
                                                                    {
                                                                        Components = new List<SceneComponentContainer>
                                                                        {
                                                                            _greifArmTransformright
                                                                        },

                                                                        Children = new List<SceneNodeContainer>
                                                                        {
                                                                            new SceneNodeContainer
                                                                            {
                                                                                Components = new List<SceneComponentContainer>
                                                                                {
                                                                                    new TransformComponent
                                                                                    {
                                                                                        Rotation = new float3(0,0,0),
                                                                                        Scale = new float3(1,1,1),
                                                                                        Translation = new float3(-0.25f,2,0)
                                                                                    },
                                                                                    // MATERIAL COMPONENT
                                                                                    new MaterialComponent
                                                                                    {
                                                                                        Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                                                                                        Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                                                                                    },

                                                                                    // MESH COMPONENT
                                                                                    SimpleMeshes.CreateCuboid(new float3(0.5f, 4, 0.5f))
                                                                                }
                                                                            },

                                                                        }

                                                                    }

                                                                }
                                                            }
                                                        },
                                                    }
                                                },
                                            }
                                        }

                                    }
                                },
                            }

                        },
                    }
                }
            };
        }
    



        


        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            int verzoegerung = 2;
            verzoegerung += verzoegerung * 10000000;
            //Rotation für den Body via Keyboard
            float bodyRot = _bodyTransform.Rotation.y;
            bodyRot += 0.05f * Keyboard.ADAxis* DeltaTime/16 * 1000;
            _bodyTransform.Rotation = new float3(0, bodyRot, 0);

            //Rotation für den upperArm und foreArm v.k.
            float uaRot = _upperArmTransform.Rotation.x;                    
            float faRot = _foreArmTransform.Rotation.x;
            var taste = Keyboard.GetKey(KeyCodes.Space);
            if(taste == true)
            {
                faRot += 0.05f * Keyboard.WSAxis * DeltaTime / 16 * 1000;
                _foreArmTransform.Rotation = new float3(faRot, 0, 0);
            }
            else
            {
                uaRot += 0.05f * Keyboard.WSAxis * DeltaTime / 16 * 1000;
                _upperArmTransform.Rotation = new float3(uaRot, 0, 0);
            } 
            //Greifarmbewegung 
            float lGaRot = _greifArmTransformleft.Rotation.z;
            float rGaRot = _greifArmTransformright.Rotation.z;
            if (Keyboard.GetKey(KeyCodes.Left) == true)
            {
                if (lGaRot <= 0.75f)
                {
                    lGaRot += 0.01f * DeltaTime / 16 * 1000;
                    _greifArmTransformleft.Rotation = new float3(0, 0, lGaRot);
                    rGaRot -= 0.01f * DeltaTime / 16 * 1000;
                    _greifArmTransformright.Rotation = new float3(0, 0, rGaRot);
                }
            }
            if (Keyboard.GetKey(KeyCodes.Right) == true)
            {
                if (lGaRot >= -0.125f)
                {
                    lGaRot -= 0.01f * DeltaTime / 16 * 1000;
                    _greifArmTransformleft.Rotation = new float3(0, 0, lGaRot);
                    rGaRot += 0.01f * DeltaTime / 16 * 1000;
                    _greifArmTransformright.Rotation = new float3(0, 0, rGaRot);
                }
            }
            

            if(Keyboard.IsKeyUp(KeyCodes.P) == true)
            {
                close = true;
            }
            

            if (close == true)
            {
                if (lGaRot >= -0.125f)
                {
                    lGaRot -= 0.015f;
                    _greifArmTransformleft.Rotation = new float3(0, 0, lGaRot);
                    rGaRot += 0.015f;
                    _greifArmTransformright.Rotation = new float3(0, 0, rGaRot);

                }
                else
                {
                    close = false;
                }
            }

            if (Keyboard.IsKeyUp(KeyCodes.O) == true)
            {
                open = true;
            }


            if (open == true)
            {
                if (lGaRot <= 0.75f)
                {
                    lGaRot += 0.015f;
                    _greifArmTransformleft.Rotation = new float3(0, 0, lGaRot);
                    rGaRot -= 0.015f;
                    _greifArmTransformright.Rotation = new float3(0, 0, rGaRot);

                }
                else
                {
                    open = false;
                }
            }

            //Kamera Bewegen mit der Maus
            var maus = Mouse.LeftButton;
            if (maus == true)
            {
                _camAngle += Mouse.Velocity.x * 0.0001f * DeltaTime / 16 * 1000;
            }

            //"zoom"-funktion 
            float achseZ = 50;
            achseZ += Mouse.Wheel ;
            
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10 , achseZ) * float4x4.CreateRotationY(_camAngle);
           


            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
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