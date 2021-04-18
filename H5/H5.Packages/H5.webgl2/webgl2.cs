// Decompiled with JetBrains decompiler
// Type: Retyped.webgl2
// Assembly: Retyped.webgl2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 36BF7521-37B1-4F4D-9780-EC3CA97E9F5A
// Assembly location: ..\retyped.webgl2.0.1.6733\lib\net40\Retyped.webgl2.dll

using H5;
using H5.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace H5.Core
{
    [Scope]
    [GlobalMethods]
    public static class webgl2
    {

        [Name("WebGL2RenderingContext")]
        public static webgl2.WebGL2RenderingContextTypeConfig WebGL2RenderingContextType
        {
            get; set;
        }

        [Name("WebGLQuery")]
        public static webgl2.WebGLQueryTypeConfig WebGLQueryType
        {
            get; set;
        }

        [Name("WebGLSampler")]
        public static webgl2.WebGLSamplerTypeConfig WebGLSamplerType
        {
            get; set;
        }

        [Name("WebGLSync")]
        public static webgl2.WebGLSyncTypeConfig WebGLSyncType
        {
            get; set;
        }

        [Name("WebGLTransformFeedback")]
        public static webgl2.WebGLTransformFeedbackTypeConfig WebGLTransformFeedbackType
        {
            get; set;
        }

        [Name("WebGLVertexArrayObject")]
        public static webgl2.WebGLVertexArrayObjectTypeConfig WebGLVertexArrayObjectType
        {
            get; set;
        }

        [IgnoreCast]
        [Virtual]
        [FormerInterface]
        public abstract class HTMLCanvasElement : dom.HTMLCanvasElement, dom.ElementCSSInlineStyle.Interface, IObject
        {
            public abstract webgl2.WebGL2RenderingContext getContext(
              webgl2.Literals.Options.contextId contextId);

            public abstract webgl2.WebGL2RenderingContext getContext(
              webgl2.Literals.Options.contextId contextId,
              dom.WebGLContextAttributes contextAttributes);

            protected HTMLCanvasElement() : base()
            {
            }
        }

        [IgnoreCast]
        [Virtual]
        [FormerInterface]
        public abstract class ImageBitmap : dom.ImageBitmap
        {
            protected ImageBitmap() : base()
            {
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class WebGL2RenderingContext : dom.WebGLRenderingContext
        {

            public static webgl2.WebGL2RenderingContext prototype { get; set; }

            public static double ACTIVE_ATTRIBUTES
            {
                get;
            }

            public static double ACTIVE_TEXTURE
            {
                get;
            }

            public static double ACTIVE_UNIFORMS
            {
                get;
            }

            public static double ALIASED_LINE_WIDTH_RANGE
            {
                get;
            }

            public static double ALIASED_POINT_SIZE_RANGE
            {
                get;
            }

            public static double ALPHA
            {
                get;
            }

            public static double ALPHA_BITS
            {
                get;
            }

            public static double ALWAYS
            {
                get;
            }

            public static double ARRAY_BUFFER
            {
                get;
            }

            public static double ARRAY_BUFFER_BINDING
            {
                get;
            }

            public static double ATTACHED_SHADERS
            {
                get;
            }

            public static double BACK
            {
                get;
            }

            public static double BLEND
            {
                get;
            }

            public static double BLEND_COLOR
            {
                get;
            }

            public static double BLEND_DST_ALPHA
            {
                get;
            }

            public static double BLEND_DST_RGB
            {
                get;
            }

            public static double BLEND_EQUATION
            {
                get;
            }

            public static double BLEND_EQUATION_ALPHA
            {
                get;
            }

            public static double BLEND_EQUATION_RGB
            {
                get;
            }

            public static double BLEND_SRC_ALPHA
            {
                get;
            }

            public static double BLEND_SRC_RGB
            {
                get;
            }

            public static double BLUE_BITS
            {
                get;
            }

            public static double BOOL
            {
                get;
            }

            public static double BOOL_VEC2
            {
                get;
            }

            public static double BOOL_VEC3
            {
                get;
            }

            public static double BOOL_VEC4
            {
                get;
            }

            public static double BROWSER_DEFAULT_WEBGL
            {
                get;
            }

            public static double BUFFER_SIZE
            {
                get;
            }

            public static double BUFFER_USAGE
            {
                get;
            }

            public static double BYTE
            {
                get;
            }

            public static double CCW
            {
                get;
            }

            public static double CLAMP_TO_EDGE
            {
                get;
            }

            public static double COLOR_ATTACHMENT0
            {
                get;
            }

            public static double COLOR_BUFFER_BIT
            {
                get;
            }

            public static double COLOR_CLEAR_VALUE
            {
                get;
            }

            public static double COLOR_WRITEMASK
            {
                get;
            }

            public static double COMPILE_STATUS
            {
                get;
            }

            public static double COMPRESSED_TEXTURE_FORMATS
            {
                get;
            }

            public static double CONSTANT_ALPHA
            {
                get;
            }

            public static double CONSTANT_COLOR
            {
                get;
            }

            public static double CONTEXT_LOST_WEBGL
            {
                get;
            }

            public static double CULL_FACE
            {
                get;
            }

            public static double CULL_FACE_MODE
            {
                get;
            }

            public static double CURRENT_PROGRAM
            {
                get;
            }

            public static double CURRENT_VERTEX_ATTRIB
            {
                get;
            }

            public static double CW
            {
                get;
            }

            public static double DECR
            {
                get;
            }

            public static double DECR_WRAP
            {
                get;
            }

            public static double DELETE_STATUS
            {
                get;
            }

            public static double DEPTH_ATTACHMENT
            {
                get;
            }

            public static double DEPTH_BITS
            {
                get;
            }

            public static double DEPTH_BUFFER_BIT
            {
                get;
            }

            public static double DEPTH_CLEAR_VALUE
            {
                get;
            }

            public static double DEPTH_COMPONENT
            {
                get;
            }

            public static double DEPTH_COMPONENT16
            {
                get;
            }

            public static double DEPTH_FUNC
            {
                get;
            }

            public static double DEPTH_RANGE
            {
                get;
            }

            public static double DEPTH_STENCIL
            {
                get;
            }

            public static double DEPTH_STENCIL_ATTACHMENT
            {
                get;
            }

            public static double DEPTH_TEST
            {
                get;
            }

            public static double DEPTH_WRITEMASK
            {
                get;
            }

            public static double DITHER
            {
                get;
            }

            public static double DONT_CARE
            {
                get;
            }

            public static double DST_ALPHA
            {
                get;
            }

            public static double DST_COLOR
            {
                get;
            }

            public static double DYNAMIC_DRAW
            {
                get;
            }

            public static double ELEMENT_ARRAY_BUFFER
            {
                get;
            }

            public static double ELEMENT_ARRAY_BUFFER_BINDING
            {
                get;
            }

            public static double EQUAL
            {
                get;
            }

            public static double FASTEST
            {
                get;
            }

            public static double FLOAT
            {
                get;
            }

            public static double FLOAT_MAT2
            {
                get;
            }

            public static double FLOAT_MAT3
            {
                get;
            }

            public static double FLOAT_MAT4
            {
                get;
            }

            public static double FLOAT_VEC2
            {
                get;
            }

            public static double FLOAT_VEC3
            {
                get;
            }

            public static double FLOAT_VEC4
            {
                get;
            }

            public static double FRAGMENT_SHADER
            {
                get;
            }

            public static double FRAMEBUFFER
            {
                get;
            }

            public static double FRAMEBUFFER_ATTACHMENT_OBJECT_NAME
            {
                get;
            }

            public static double FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE
            {
                get;
            }

            public static double FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE
            {
                get;
            }

            public static double FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL
            {
                get;
            }

            public static double FRAMEBUFFER_BINDING
            {
                get;
            }

            public static double FRAMEBUFFER_COMPLETE
            {
                get;
            }

            public static double FRAMEBUFFER_INCOMPLETE_ATTACHMENT
            {
                get;
            }

            public static double FRAMEBUFFER_INCOMPLETE_DIMENSIONS
            {
                get;
            }

            public static double FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT
            {
                get;
            }

            public static double FRAMEBUFFER_UNSUPPORTED
            {
                get;
            }

            public static double FRONT
            {
                get;
            }

            public static double FRONT_AND_BACK
            {
                get;
            }

            public static double FRONT_FACE
            {
                get;
            }

            public static double FUNC_ADD
            {
                get;
            }

            public static double FUNC_REVERSE_SUBTRACT
            {
                get;
            }

            public static double FUNC_SUBTRACT
            {
                get;
            }

            public static double GENERATE_MIPMAP_HINT
            {
                get;
            }

            public static double GEQUAL
            {
                get;
            }

            public static double GREATER
            {
                get;
            }

            public static double GREEN_BITS
            {
                get;
            }

            public static double HIGH_FLOAT
            {
                get;
            }

            public static double HIGH_INT
            {
                get;
            }

            public static double IMPLEMENTATION_COLOR_READ_FORMAT
            {
                get;
            }

            public static double IMPLEMENTATION_COLOR_READ_TYPE
            {
                get;
            }

            public static double INCR
            {
                get;
            }

            public static double INCR_WRAP
            {
                get;
            }

            public static double INT
            {
                get;
            }

            public static double INT_VEC2
            {
                get;
            }

            public static double INT_VEC3
            {
                get;
            }

            public static double INT_VEC4
            {
                get;
            }

            public static double INVALID_ENUM
            {
                get;
            }

            public static double INVALID_FRAMEBUFFER_OPERATION
            {
                get;
            }

            public static double INVALID_OPERATION
            {
                get;
            }

            public static double INVALID_VALUE
            {
                get;
            }

            public static double INVERT
            {
                get;
            }

            public static double KEEP
            {
                get;
            }

            public static double LEQUAL
            {
                get;
            }

            public static double LESS
            {
                get;
            }

            public static double LINEAR
            {
                get;
            }

            public static double LINEAR_MIPMAP_LINEAR
            {
                get;
            }

            public static double LINEAR_MIPMAP_NEAREST
            {
                get;
            }

            public static double LINES
            {
                get;
            }

            public static double LINE_LOOP
            {
                get;
            }

            public static double LINE_STRIP
            {
                get;
            }

            public static double LINE_WIDTH
            {
                get;
            }

            public static double LINK_STATUS
            {
                get;
            }

            public static double LOW_FLOAT
            {
                get;
            }

            public static double LOW_INT
            {
                get;
            }

            public static double LUMINANCE
            {
                get;
            }

            public static double LUMINANCE_ALPHA
            {
                get;
            }

            public static double MAX_COMBINED_TEXTURE_IMAGE_UNITS
            {
                get;
            }

            public static double MAX_CUBE_MAP_TEXTURE_SIZE
            {
                get;
            }

            public static double MAX_FRAGMENT_UNIFORM_VECTORS
            {
                get;
            }

            public static double MAX_RENDERBUFFER_SIZE
            {
                get;
            }

            public static double MAX_TEXTURE_IMAGE_UNITS
            {
                get;
            }

            public static double MAX_TEXTURE_SIZE
            {
                get;
            }

            public static double MAX_VARYING_VECTORS
            {
                get;
            }

            public static double MAX_VERTEX_ATTRIBS
            {
                get;
            }

            public static double MAX_VERTEX_TEXTURE_IMAGE_UNITS
            {
                get;
            }

            public static double MAX_VERTEX_UNIFORM_VECTORS
            {
                get;
            }

            public static double MAX_VIEWPORT_DIMS
            {
                get;
            }

            public static double MEDIUM_FLOAT
            {
                get;
            }

            public static double MEDIUM_INT
            {
                get;
            }

            public static double MIRRORED_REPEAT
            {
                get;
            }

            public static double NEAREST
            {
                get;
            }

            public static double NEAREST_MIPMAP_LINEAR
            {
                get;
            }

            public static double NEAREST_MIPMAP_NEAREST
            {
                get;
            }

            public static double NEVER
            {
                get;
            }

            public static double NICEST
            {
                get;
            }

            public static double NONE
            {
                get;
            }

            public static double NOTEQUAL
            {
                get;
            }

            public static double NO_ERROR
            {
                get;
            }

            public static double ONE
            {
                get;
            }

            public static double ONE_MINUS_CONSTANT_ALPHA
            {
                get;
            }

            public static double ONE_MINUS_CONSTANT_COLOR
            {
                get;
            }

            public static double ONE_MINUS_DST_ALPHA
            {
                get;
            }

            public static double ONE_MINUS_DST_COLOR
            {
                get;
            }

            public static double ONE_MINUS_SRC_ALPHA
            {
                get;
            }

            public static double ONE_MINUS_SRC_COLOR
            {
                get;
            }

            public static double OUT_OF_MEMORY
            {
                get;
            }

            public static double PACK_ALIGNMENT
            {
                get;
            }

            public static double POINTS
            {
                get;
            }

            public static double POLYGON_OFFSET_FACTOR
            {
                get;
            }

            public static double POLYGON_OFFSET_FILL
            {
                get;
            }

            public static double POLYGON_OFFSET_UNITS
            {
                get;
            }

            public static double RED_BITS
            {
                get;
            }

            public static double RENDERBUFFER
            {
                get;
            }

            public static double RENDERBUFFER_ALPHA_SIZE
            {
                get;
            }

            public static double RENDERBUFFER_BINDING
            {
                get;
            }

            public static double RENDERBUFFER_BLUE_SIZE
            {
                get;
            }

            public static double RENDERBUFFER_DEPTH_SIZE
            {
                get;
            }

            public static double RENDERBUFFER_GREEN_SIZE
            {
                get;
            }

            public static double RENDERBUFFER_HEIGHT
            {
                get;
            }

            public static double RENDERBUFFER_INTERNAL_FORMAT
            {
                get;
            }

            public static double RENDERBUFFER_RED_SIZE
            {
                get;
            }

            public static double RENDERBUFFER_STENCIL_SIZE
            {
                get;
            }

            public static double RENDERBUFFER_WIDTH
            {
                get;
            }

            public static double RENDERER
            {
                get;
            }

            public static double REPEAT
            {
                get;
            }

            public static double REPLACE
            {
                get;
            }

            public static double RGB
            {
                get;
            }

            public static double RGB565
            {
                get;
            }

            public static double RGB5_A1
            {
                get;
            }

            public static double RGBA
            {
                get;
            }

            public static double RGBA4
            {
                get;
            }

            public static double SAMPLER_2D
            {
                get;
            }

            public static double SAMPLER_CUBE
            {
                get;
            }

            public static double SAMPLES
            {
                get;
            }

            public static double SAMPLE_ALPHA_TO_COVERAGE
            {
                get;
            }

            public static double SAMPLE_BUFFERS
            {
                get;
            }

            public static double SAMPLE_COVERAGE
            {
                get;
            }

            public static double SAMPLE_COVERAGE_INVERT
            {
                get;
            }

            public static double SAMPLE_COVERAGE_VALUE
            {
                get;
            }

            public static double SCISSOR_BOX
            {
                get;
            }

            public static double SCISSOR_TEST
            {
                get;
            }

            public static double SHADER_TYPE
            {
                get;
            }

            public static double SHADING_LANGUAGE_VERSION
            {
                get;
            }

            public static double SHORT
            {
                get;
            }

            public static double SRC_ALPHA
            {
                get;
            }

            public static double SRC_ALPHA_SATURATE
            {
                get;
            }

            public static double SRC_COLOR
            {
                get;
            }

            public static double STATIC_DRAW
            {
                get;
            }

            public static double STENCIL_ATTACHMENT
            {
                get;
            }

            public static double STENCIL_BACK_FAIL
            {
                get;
            }

            public static double STENCIL_BACK_FUNC
            {
                get;
            }

            public static double STENCIL_BACK_PASS_DEPTH_FAIL
            {
                get;
            }

            public static double STENCIL_BACK_PASS_DEPTH_PASS
            {
                get;
            }

            public static double STENCIL_BACK_REF
            {
                get;
            }

            public static double STENCIL_BACK_VALUE_MASK
            {
                get;
            }

            public static double STENCIL_BACK_WRITEMASK
            {
                get;
            }

            public static double STENCIL_BITS
            {
                get;
            }

            public static double STENCIL_BUFFER_BIT
            {
                get;
            }

            public static double STENCIL_CLEAR_VALUE
            {
                get;
            }

            public static double STENCIL_FAIL
            {
                get;
            }

            public static double STENCIL_FUNC
            {
                get;
            }

            public static double STENCIL_INDEX
            {
                get;
            }

            public static double STENCIL_INDEX8
            {
                get;
            }

            public static double STENCIL_PASS_DEPTH_FAIL
            {
                get;
            }

            public static double STENCIL_PASS_DEPTH_PASS
            {
                get;
            }

            public static double STENCIL_REF
            {
                get;
            }

            public static double STENCIL_TEST
            {
                get;
            }

            public static double STENCIL_VALUE_MASK
            {
                get;
            }

            public static double STENCIL_WRITEMASK
            {
                get;
            }

            public static double STREAM_DRAW
            {
                get;
            }

            public static double SUBPIXEL_BITS
            {
                get;
            }

            public static double TEXTURE
            {
                get;
            }

            public static double TEXTURE0
            {
                get;
            }

            public static double TEXTURE1
            {
                get;
            }

            public static double TEXTURE10
            {
                get;
            }

            public static double TEXTURE11
            {
                get;
            }

            public static double TEXTURE12
            {
                get;
            }

            public static double TEXTURE13
            {
                get;
            }

            public static double TEXTURE14
            {
                get;
            }

            public static double TEXTURE15
            {
                get;
            }

            public static double TEXTURE16
            {
                get;
            }

            public static double TEXTURE17
            {
                get;
            }

            public static double TEXTURE18
            {
                get;
            }

            public static double TEXTURE19
            {
                get;
            }

            public static double TEXTURE2
            {
                get;
            }

            public static double TEXTURE20
            {
                get;
            }

            public static double TEXTURE21
            {
                get;
            }

            public static double TEXTURE22
            {
                get;
            }

            public static double TEXTURE23
            {
                get;
            }

            public static double TEXTURE24
            {
                get;
            }

            public static double TEXTURE25
            {
                get;
            }

            public static double TEXTURE26
            {
                get;
            }

            public static double TEXTURE27
            {
                get;
            }

            public static double TEXTURE28
            {
                get;
            }

            public static double TEXTURE29
            {
                get;
            }

            public static double TEXTURE3
            {
                get;
            }

            public static double TEXTURE30
            {
                get;
            }

            public static double TEXTURE31
            {
                get;
            }

            public static double TEXTURE4
            {
                get;
            }

            public static double TEXTURE5
            {
                get;
            }

            public static double TEXTURE6
            {
                get;
            }

            public static double TEXTURE7
            {
                get;
            }

            public static double TEXTURE8
            {
                get;
            }

            public static double TEXTURE9
            {
                get;
            }

            public static double TEXTURE_2D
            {
                get;
            }

            public static double TEXTURE_BINDING_2D
            {
                get;
            }

            public static double TEXTURE_BINDING_CUBE_MAP
            {
                get;
            }

            public static double TEXTURE_CUBE_MAP
            {
                get;
            }

            public static double TEXTURE_CUBE_MAP_NEGATIVE_X
            {
                get;
            }

            public static double TEXTURE_CUBE_MAP_NEGATIVE_Y
            {
                get;
            }

            public static double TEXTURE_CUBE_MAP_NEGATIVE_Z
            {
                get;
            }

            public static double TEXTURE_CUBE_MAP_POSITIVE_X
            {
                get;
            }

            public static double TEXTURE_CUBE_MAP_POSITIVE_Y
            {
                get;
            }

            public static double TEXTURE_CUBE_MAP_POSITIVE_Z
            {
                get;
            }

            public static double TEXTURE_MAG_FILTER
            {
                get;
            }

            public static double TEXTURE_MIN_FILTER
            {
                get;
            }

            public static double TEXTURE_WRAP_S
            {
                get;
            }

            public static double TEXTURE_WRAP_T
            {
                get;
            }

            public static double TRIANGLES
            {
                get;
            }

            public static double TRIANGLE_FAN
            {
                get;
            }

            public static double TRIANGLE_STRIP
            {
                get;
            }

            public static double UNPACK_ALIGNMENT
            {
                get;
            }

            public static double UNPACK_COLORSPACE_CONVERSION_WEBGL
            {
                get;
            }

            public static double UNPACK_FLIP_Y_WEBGL
            {
                get;
            }

            public static double UNPACK_PREMULTIPLY_ALPHA_WEBGL
            {
                get;
            }

            public static double UNSIGNED_BYTE
            {
                get;
            }

            public static double UNSIGNED_INT
            {
                get;
            }

            public static double UNSIGNED_SHORT
            {
                get;
            }

            public static double UNSIGNED_SHORT_4_4_4_4
            {
                get;
            }

            public static double UNSIGNED_SHORT_5_5_5_1
            {
                get;
            }

            public static double UNSIGNED_SHORT_5_6_5
            {
                get;
            }

            public static double VALIDATE_STATUS
            {
                get;
            }

            public static double VENDOR
            {
                get;
            }

            public static double VERSION
            {
                get;
            }

            public static double VERTEX_ATTRIB_ARRAY_BUFFER_BINDING
            {
                get;
            }

            public static double VERTEX_ATTRIB_ARRAY_ENABLED
            {
                get;
            }

            public static double VERTEX_ATTRIB_ARRAY_NORMALIZED
            {
                get;
            }

            public static double VERTEX_ATTRIB_ARRAY_POINTER
            {
                get;
            }

            public static double VERTEX_ATTRIB_ARRAY_SIZE
            {
                get;
            }

            public static double VERTEX_ATTRIB_ARRAY_STRIDE
            {
                get;
            }

            public static double VERTEX_ATTRIB_ARRAY_TYPE
            {
                get;
            }

            public static double VERTEX_SHADER
            {
                get;
            }

            public static double VIEWPORT
            {
                get;
            }

            public static double ZERO
            {
                get;
            }

            [Name("READ_BUFFER")]
            public static double READ_BUFFER_Static
            {
                get;
            }

            [Name("UNPACK_ROW_LENGTH")]
            public static double UNPACK_ROW_LENGTH_Static
            {
                get;
            }

            [Name("UNPACK_SKIP_ROWS")]
            public static double UNPACK_SKIP_ROWS_Static
            {
                get;
            }

            [Name("UNPACK_SKIP_PIXELS")]
            public static double UNPACK_SKIP_PIXELS_Static
            {
                get;
            }

            [Name("PACK_ROW_LENGTH")]
            public static double PACK_ROW_LENGTH_Static
            {
                get;
            }

            [Name("PACK_SKIP_ROWS")]
            public static double PACK_SKIP_ROWS_Static
            {
                get;
            }

            [Name("PACK_SKIP_PIXELS")]
            public static double PACK_SKIP_PIXELS_Static
            {
                get;
            }

            [Name("COLOR")]
            public static double COLOR_Static
            {
                get;
            }

            [Name("DEPTH")]
            public static double DEPTH_Static
            {
                get;
            }

            [Name("STENCIL")]
            public static double STENCIL_Static
            {
                get;
            }

            [Name("RED")]
            public static double RED_Static
            {
                get;
            }

            [Name("RGB8")]
            public static double RGB8_Static
            {
                get;
            }

            [Name("RGBA8")]
            public static double RGBA8_Static
            {
                get;
            }

            [Name("RGB10_A2")]
            public static double RGB10_A2_Static
            {
                get;
            }

            [Name("TEXTURE_BINDING_3D")]
            public static double TEXTURE_BINDING_3D_Static
            {
                get;
            }

            [Name("UNPACK_SKIP_IMAGES")]
            public static double UNPACK_SKIP_IMAGES_Static
            {
                get;
            }

            [Name("UNPACK_IMAGE_HEIGHT")]
            public static double UNPACK_IMAGE_HEIGHT_Static
            {
                get;
            }

            [Name("TEXTURE_3D")]
            public static double TEXTURE_3D_Static
            {
                get;
            }

            [Name("TEXTURE_WRAP_R")]
            public static double TEXTURE_WRAP_R_Static
            {
                get;
            }

            [Name("MAX_3D_TEXTURE_SIZE")]
            public static double MAX_3D_TEXTURE_SIZE_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_2_10_10_10_REV")]
            public static double UNSIGNED_INT_2_10_10_10_REV_Static
            {
                get;
            }

            [Name("MAX_ELEMENTS_VERTICES")]
            public static double MAX_ELEMENTS_VERTICES_Static
            {
                get;
            }

            [Name("MAX_ELEMENTS_INDICES")]
            public static double MAX_ELEMENTS_INDICES_Static
            {
                get;
            }

            [Name("TEXTURE_MIN_LOD")]
            public static double TEXTURE_MIN_LOD_Static
            {
                get;
            }

            [Name("TEXTURE_MAX_LOD")]
            public static double TEXTURE_MAX_LOD_Static
            {
                get;
            }

            [Name("TEXTURE_BASE_LEVEL")]
            public static double TEXTURE_BASE_LEVEL_Static
            {
                get;
            }

            [Name("TEXTURE_MAX_LEVEL")]
            public static double TEXTURE_MAX_LEVEL_Static
            {
                get;
            }

            [Name("MIN")]
            public static double MIN_Static
            {
                get;
            }

            [Name("MAX")]
            public static double MAX_Static
            {
                get;
            }

            [Name("DEPTH_COMPONENT24")]
            public static double DEPTH_COMPONENT24_Static
            {
                get;
            }

            [Name("MAX_TEXTURE_LOD_BIAS")]
            public static double MAX_TEXTURE_LOD_BIAS_Static
            {
                get;
            }

            [Name("TEXTURE_COMPARE_MODE")]
            public static double TEXTURE_COMPARE_MODE_Static
            {
                get;
            }

            [Name("TEXTURE_COMPARE_FUNC")]
            public static double TEXTURE_COMPARE_FUNC_Static
            {
                get;
            }

            [Name("CURRENT_QUERY")]
            public static double CURRENT_QUERY_Static
            {
                get;
            }

            [Name("QUERY_RESULT")]
            public static double QUERY_RESULT_Static
            {
                get;
            }

            [Name("QUERY_RESULT_AVAILABLE")]
            public static double QUERY_RESULT_AVAILABLE_Static
            {
                get;
            }

            [Name("STREAM_READ")]
            public static double STREAM_READ_Static
            {
                get;
            }

            [Name("STREAM_COPY")]
            public static double STREAM_COPY_Static
            {
                get;
            }

            [Name("STATIC_READ")]
            public static double STATIC_READ_Static
            {
                get;
            }

            [Name("STATIC_COPY")]
            public static double STATIC_COPY_Static
            {
                get;
            }

            [Name("DYNAMIC_READ")]
            public static double DYNAMIC_READ_Static
            {
                get;
            }

            [Name("DYNAMIC_COPY")]
            public static double DYNAMIC_COPY_Static
            {
                get;
            }

            [Name("MAX_DRAW_BUFFERS")]
            public static double MAX_DRAW_BUFFERS_Static
            {
                get;
            }

            [Name("DRAW_BUFFER0")]
            public static double DRAW_BUFFER0_Static
            {
                get;
            }

            [Name("DRAW_BUFFER1")]
            public static double DRAW_BUFFER1_Static
            {
                get;
            }

            [Name("DRAW_BUFFER2")]
            public static double DRAW_BUFFER2_Static
            {
                get;
            }

            [Name("DRAW_BUFFER3")]
            public static double DRAW_BUFFER3_Static
            {
                get;
            }

            [Name("DRAW_BUFFER4")]
            public static double DRAW_BUFFER4_Static
            {
                get;
            }

            [Name("DRAW_BUFFER5")]
            public static double DRAW_BUFFER5_Static
            {
                get;
            }

            [Name("DRAW_BUFFER6")]
            public static double DRAW_BUFFER6_Static
            {
                get;
            }

            [Name("DRAW_BUFFER7")]
            public static double DRAW_BUFFER7_Static
            {
                get;
            }

            [Name("DRAW_BUFFER8")]
            public static double DRAW_BUFFER8_Static
            {
                get;
            }

            [Name("DRAW_BUFFER9")]
            public static double DRAW_BUFFER9_Static
            {
                get;
            }

            [Name("DRAW_BUFFER10")]
            public static double DRAW_BUFFER10_Static
            {
                get;
            }

            [Name("DRAW_BUFFER11")]
            public static double DRAW_BUFFER11_Static
            {
                get;
            }

            [Name("DRAW_BUFFER12")]
            public static double DRAW_BUFFER12_Static
            {
                get;
            }

            [Name("DRAW_BUFFER13")]
            public static double DRAW_BUFFER13_Static
            {
                get;
            }

            [Name("DRAW_BUFFER14")]
            public static double DRAW_BUFFER14_Static
            {
                get;
            }

            [Name("DRAW_BUFFER15")]
            public static double DRAW_BUFFER15_Static
            {
                get;
            }

            [Name("MAX_FRAGMENT_UNIFORM_COMPONENTS")]
            public static double MAX_FRAGMENT_UNIFORM_COMPONENTS_Static
            {
                get;
            }

            [Name("MAX_VERTEX_UNIFORM_COMPONENTS")]
            public static double MAX_VERTEX_UNIFORM_COMPONENTS_Static
            {
                get;
            }

            [Name("SAMPLER_3D")]
            public static double SAMPLER_3D_Static
            {
                get;
            }

            [Name("SAMPLER_2D_SHADOW")]
            public static double SAMPLER_2D_SHADOW_Static
            {
                get;
            }

            [Name("FRAGMENT_SHADER_DERIVATIVE_HINT")]
            public static double FRAGMENT_SHADER_DERIVATIVE_HINT_Static
            {
                get;
            }

            [Name("PIXEL_PACK_BUFFER")]
            public static double PIXEL_PACK_BUFFER_Static
            {
                get;
            }

            [Name("PIXEL_UNPACK_BUFFER")]
            public static double PIXEL_UNPACK_BUFFER_Static
            {
                get;
            }

            [Name("PIXEL_PACK_BUFFER_BINDING")]
            public static double PIXEL_PACK_BUFFER_BINDING_Static
            {
                get;
            }

            [Name("PIXEL_UNPACK_BUFFER_BINDING")]
            public static double PIXEL_UNPACK_BUFFER_BINDING_Static
            {
                get;
            }

            [Name("FLOAT_MAT2x3")]
            public static double FLOAT_MAT2x3_Static
            {
                get;
            }

            [Name("FLOAT_MAT2x4")]
            public static double FLOAT_MAT2x4_Static
            {
                get;
            }

            [Name("FLOAT_MAT3x2")]
            public static double FLOAT_MAT3x2_Static
            {
                get;
            }

            [Name("FLOAT_MAT3x4")]
            public static double FLOAT_MAT3x4_Static
            {
                get;
            }

            [Name("FLOAT_MAT4x2")]
            public static double FLOAT_MAT4x2_Static
            {
                get;
            }

            [Name("FLOAT_MAT4x3")]
            public static double FLOAT_MAT4x3_Static
            {
                get;
            }

            [Name("SRGB")]
            public static double SRGB_Static
            {
                get;
            }

            [Name("SRGB8")]
            public static double SRGB8_Static
            {
                get;
            }

            [Name("SRGB8_ALPHA8")]
            public static double SRGB8_ALPHA8_Static
            {
                get;
            }

            [Name("COMPARE_REF_TO_TEXTURE")]
            public static double COMPARE_REF_TO_TEXTURE_Static
            {
                get;
            }

            [Name("RGBA32F")]
            public static double RGBA32F_Static
            {
                get;
            }

            [Name("RGB32F")]
            public static double RGB32F_Static
            {
                get;
            }

            [Name("RGBA16F")]
            public static double RGBA16F_Static
            {
                get;
            }

            [Name("RGB16F")]
            public static double RGB16F_Static
            {
                get;
            }

            [Name("VERTEX_ATTRIB_ARRAY_INTEGER")]
            public static double VERTEX_ATTRIB_ARRAY_INTEGER_Static
            {
                get;
            }

            [Name("MAX_ARRAY_TEXTURE_LAYERS")]
            public static double MAX_ARRAY_TEXTURE_LAYERS_Static
            {
                get;
            }

            [Name("MIN_PROGRAM_TEXEL_OFFSET")]
            public static double MIN_PROGRAM_TEXEL_OFFSET_Static
            {
                get;
            }

            [Name("MAX_PROGRAM_TEXEL_OFFSET")]
            public static double MAX_PROGRAM_TEXEL_OFFSET_Static
            {
                get;
            }

            [Name("MAX_VARYING_COMPONENTS")]
            public static double MAX_VARYING_COMPONENTS_Static
            {
                get;
            }

            [Name("TEXTURE_2D_ARRAY")]
            public static double TEXTURE_2D_ARRAY_Static
            {
                get;
            }

            [Name("TEXTURE_BINDING_2D_ARRAY")]
            public static double TEXTURE_BINDING_2D_ARRAY_Static
            {
                get;
            }

            [Name("R11F_G11F_B10F")]
            public static double R11F_G11F_B10F_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_10F_11F_11F_REV")]
            public static double UNSIGNED_INT_10F_11F_11F_REV_Static
            {
                get;
            }

            [Name("RGB9_E5")]
            public static double RGB9_E5_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_5_9_9_9_REV")]
            public static double UNSIGNED_INT_5_9_9_9_REV_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_BUFFER_MODE")]
            public static double TRANSFORM_FEEDBACK_BUFFER_MODE_Static
            {
                get;
            }

            [Name("MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS")]
            public static double MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_VARYINGS")]
            public static double TRANSFORM_FEEDBACK_VARYINGS_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_BUFFER_START")]
            public static double TRANSFORM_FEEDBACK_BUFFER_START_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_BUFFER_SIZE")]
            public static double TRANSFORM_FEEDBACK_BUFFER_SIZE_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN")]
            public static double TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN_Static
            {
                get;
            }

            [Name("RASTERIZER_DISCARD")]
            public static double RASTERIZER_DISCARD_Static
            {
                get;
            }

            [Name("MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS")]
            public static double MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS_Static
            {
                get;
            }

            [Name("MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS")]
            public static double MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS_Static
            {
                get;
            }

            [Name("INTERLEAVED_ATTRIBS")]
            public static double INTERLEAVED_ATTRIBS_Static
            {
                get;
            }

            [Name("SEPARATE_ATTRIBS")]
            public static double SEPARATE_ATTRIBS_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_BUFFER")]
            public static double TRANSFORM_FEEDBACK_BUFFER_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_BUFFER_BINDING")]
            public static double TRANSFORM_FEEDBACK_BUFFER_BINDING_Static
            {
                get;
            }

            [Name("RGBA32UI")]
            public static double RGBA32UI_Static
            {
                get;
            }

            [Name("RGB32UI")]
            public static double RGB32UI_Static
            {
                get;
            }

            [Name("RGBA16UI")]
            public static double RGBA16UI_Static
            {
                get;
            }

            [Name("RGB16UI")]
            public static double RGB16UI_Static
            {
                get;
            }

            [Name("RGBA8UI")]
            public static double RGBA8UI_Static
            {
                get;
            }

            [Name("RGB8UI")]
            public static double RGB8UI_Static
            {
                get;
            }

            [Name("RGBA32I")]
            public static double RGBA32I_Static
            {
                get;
            }

            [Name("RGB32I")]
            public static double RGB32I_Static
            {
                get;
            }

            [Name("RGBA16I")]
            public static double RGBA16I_Static
            {
                get;
            }

            [Name("RGB16I")]
            public static double RGB16I_Static
            {
                get;
            }

            [Name("RGBA8I")]
            public static double RGBA8I_Static
            {
                get;
            }

            [Name("RGB8I")]
            public static double RGB8I_Static
            {
                get;
            }

            [Name("RED_INTEGER")]
            public static double RED_INTEGER_Static
            {
                get;
            }

            [Name("RGB_INTEGER")]
            public static double RGB_INTEGER_Static
            {
                get;
            }

            [Name("RGBA_INTEGER")]
            public static double RGBA_INTEGER_Static
            {
                get;
            }

            [Name("SAMPLER_2D_ARRAY")]
            public static double SAMPLER_2D_ARRAY_Static
            {
                get;
            }

            [Name("SAMPLER_2D_ARRAY_SHADOW")]
            public static double SAMPLER_2D_ARRAY_SHADOW_Static
            {
                get;
            }

            [Name("SAMPLER_CUBE_SHADOW")]
            public static double SAMPLER_CUBE_SHADOW_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_VEC2")]
            public static double UNSIGNED_INT_VEC2_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_VEC3")]
            public static double UNSIGNED_INT_VEC3_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_VEC4")]
            public static double UNSIGNED_INT_VEC4_Static
            {
                get;
            }

            [Name("INT_SAMPLER_2D")]
            public static double INT_SAMPLER_2D_Static
            {
                get;
            }

            [Name("INT_SAMPLER_3D")]
            public static double INT_SAMPLER_3D_Static
            {
                get;
            }

            [Name("INT_SAMPLER_CUBE")]
            public static double INT_SAMPLER_CUBE_Static
            {
                get;
            }

            [Name("INT_SAMPLER_2D_ARRAY")]
            public static double INT_SAMPLER_2D_ARRAY_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_SAMPLER_2D")]
            public static double UNSIGNED_INT_SAMPLER_2D_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_SAMPLER_3D")]
            public static double UNSIGNED_INT_SAMPLER_3D_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_SAMPLER_CUBE")]
            public static double UNSIGNED_INT_SAMPLER_CUBE_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_SAMPLER_2D_ARRAY")]
            public static double UNSIGNED_INT_SAMPLER_2D_ARRAY_Static
            {
                get;
            }

            [Name("DEPTH_COMPONENT32F")]
            public static double DEPTH_COMPONENT32F_Static
            {
                get;
            }

            [Name("DEPTH32F_STENCIL8")]
            public static double DEPTH32F_STENCIL8_Static
            {
                get;
            }

            [Name("FLOAT_32_UNSIGNED_INT_24_8_REV")]
            public static double FLOAT_32_UNSIGNED_INT_24_8_REV_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING")]
            public static double FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE")]
            public static double FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_RED_SIZE")]
            public static double FRAMEBUFFER_ATTACHMENT_RED_SIZE_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_GREEN_SIZE")]
            public static double FRAMEBUFFER_ATTACHMENT_GREEN_SIZE_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_BLUE_SIZE")]
            public static double FRAMEBUFFER_ATTACHMENT_BLUE_SIZE_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE")]
            public static double FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE")]
            public static double FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE")]
            public static double FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_DEFAULT")]
            public static double FRAMEBUFFER_DEFAULT_Static
            {
                get;
            }

            [Name("UNSIGNED_INT_24_8")]
            public static double UNSIGNED_INT_24_8_Static
            {
                get;
            }

            [Name("DEPTH24_STENCIL8")]
            public static double DEPTH24_STENCIL8_Static
            {
                get;
            }

            [Name("UNSIGNED_NORMALIZED")]
            public static double UNSIGNED_NORMALIZED_Static
            {
                get;
            }

            [Name("DRAW_FRAMEBUFFER_BINDING")]
            public static double DRAW_FRAMEBUFFER_BINDING_Static
            {
                get;
            }

            [Name("READ_FRAMEBUFFER")]
            public static double READ_FRAMEBUFFER_Static
            {
                get;
            }

            [Name("DRAW_FRAMEBUFFER")]
            public static double DRAW_FRAMEBUFFER_Static
            {
                get;
            }

            [Name("READ_FRAMEBUFFER_BINDING")]
            public static double READ_FRAMEBUFFER_BINDING_Static
            {
                get;
            }

            [Name("RENDERBUFFER_SAMPLES")]
            public static double RENDERBUFFER_SAMPLES_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER")]
            public static double FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER_Static
            {
                get;
            }

            [Name("MAX_COLOR_ATTACHMENTS")]
            public static double MAX_COLOR_ATTACHMENTS_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT1")]
            public static double COLOR_ATTACHMENT1_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT2")]
            public static double COLOR_ATTACHMENT2_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT3")]
            public static double COLOR_ATTACHMENT3_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT4")]
            public static double COLOR_ATTACHMENT4_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT5")]
            public static double COLOR_ATTACHMENT5_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT6")]
            public static double COLOR_ATTACHMENT6_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT7")]
            public static double COLOR_ATTACHMENT7_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT8")]
            public static double COLOR_ATTACHMENT8_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT9")]
            public static double COLOR_ATTACHMENT9_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT10")]
            public static double COLOR_ATTACHMENT10_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT11")]
            public static double COLOR_ATTACHMENT11_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT12")]
            public static double COLOR_ATTACHMENT12_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT13")]
            public static double COLOR_ATTACHMENT13_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT14")]
            public static double COLOR_ATTACHMENT14_Static
            {
                get;
            }

            [Name("COLOR_ATTACHMENT15")]
            public static double COLOR_ATTACHMENT15_Static
            {
                get;
            }

            [Name("FRAMEBUFFER_INCOMPLETE_MULTISAMPLE")]
            public static double FRAMEBUFFER_INCOMPLETE_MULTISAMPLE_Static
            {
                get;
            }

            [Name("MAX_SAMPLES")]
            public static double MAX_SAMPLES_Static
            {
                get;
            }

            [Name("HALF_FLOAT")]
            public static double HALF_FLOAT_Static
            {
                get;
            }

            [Name("RG")]
            public static double RG_Static
            {
                get;
            }

            [Name("RG_INTEGER")]
            public static double RG_INTEGER_Static
            {
                get;
            }

            [Name("R8")]
            public static double R8_Static
            {
                get;
            }

            [Name("RG8")]
            public static double RG8_Static
            {
                get;
            }

            [Name("R16F")]
            public static double R16F_Static
            {
                get;
            }

            [Name("R32F")]
            public static double R32F_Static
            {
                get;
            }

            [Name("RG16F")]
            public static double RG16F_Static
            {
                get;
            }

            [Name("RG32F")]
            public static double RG32F_Static
            {
                get;
            }

            [Name("R8I")]
            public static double R8I_Static
            {
                get;
            }

            [Name("R8UI")]
            public static double R8UI_Static
            {
                get;
            }

            [Name("R16I")]
            public static double R16I_Static
            {
                get;
            }

            [Name("R16UI")]
            public static double R16UI_Static
            {
                get;
            }

            [Name("R32I")]
            public static double R32I_Static
            {
                get;
            }

            [Name("R32UI")]
            public static double R32UI_Static
            {
                get;
            }

            [Name("RG8I")]
            public static double RG8I_Static
            {
                get;
            }

            [Name("RG8UI")]
            public static double RG8UI_Static
            {
                get;
            }

            [Name("RG16I")]
            public static double RG16I_Static
            {
                get;
            }

            [Name("RG16UI")]
            public static double RG16UI_Static
            {
                get;
            }

            [Name("RG32I")]
            public static double RG32I_Static
            {
                get;
            }

            [Name("RG32UI")]
            public static double RG32UI_Static
            {
                get;
            }

            [Name("VERTEX_ARRAY_BINDING")]
            public static double VERTEX_ARRAY_BINDING_Static
            {
                get;
            }

            [Name("R8_SNORM")]
            public static double R8_SNORM_Static
            {
                get;
            }

            [Name("RG8_SNORM")]
            public static double RG8_SNORM_Static
            {
                get;
            }

            [Name("RGB8_SNORM")]
            public static double RGB8_SNORM_Static
            {
                get;
            }

            [Name("RGBA8_SNORM")]
            public static double RGBA8_SNORM_Static
            {
                get;
            }

            [Name("SIGNED_NORMALIZED")]
            public static double SIGNED_NORMALIZED_Static
            {
                get;
            }

            [Name("COPY_READ_BUFFER")]
            public static double COPY_READ_BUFFER_Static
            {
                get;
            }

            [Name("COPY_WRITE_BUFFER")]
            public static double COPY_WRITE_BUFFER_Static
            {
                get;
            }

            [Name("COPY_READ_BUFFER_BINDING")]
            public static double COPY_READ_BUFFER_BINDING_Static
            {
                get;
            }

            [Name("COPY_WRITE_BUFFER_BINDING")]
            public static double COPY_WRITE_BUFFER_BINDING_Static
            {
                get;
            }

            [Name("UNIFORM_BUFFER")]
            public static double UNIFORM_BUFFER_Static
            {
                get;
            }

            [Name("UNIFORM_BUFFER_BINDING")]
            public static double UNIFORM_BUFFER_BINDING_Static
            {
                get;
            }

            [Name("UNIFORM_BUFFER_START")]
            public static double UNIFORM_BUFFER_START_Static
            {
                get;
            }

            [Name("UNIFORM_BUFFER_SIZE")]
            public static double UNIFORM_BUFFER_SIZE_Static
            {
                get;
            }

            [Name("MAX_VERTEX_UNIFORM_BLOCKS")]
            public static double MAX_VERTEX_UNIFORM_BLOCKS_Static
            {
                get;
            }

            [Name("MAX_FRAGMENT_UNIFORM_BLOCKS")]
            public static double MAX_FRAGMENT_UNIFORM_BLOCKS_Static
            {
                get;
            }

            [Name("MAX_COMBINED_UNIFORM_BLOCKS")]
            public static double MAX_COMBINED_UNIFORM_BLOCKS_Static
            {
                get;
            }

            [Name("MAX_UNIFORM_BUFFER_BINDINGS")]
            public static double MAX_UNIFORM_BUFFER_BINDINGS_Static
            {
                get;
            }

            [Name("MAX_UNIFORM_BLOCK_SIZE")]
            public static double MAX_UNIFORM_BLOCK_SIZE_Static
            {
                get;
            }

            [Name("MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS")]
            public static double MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS_Static
            {
                get;
            }

            [Name("MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS")]
            public static double MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS_Static
            {
                get;
            }

            [Name("UNIFORM_BUFFER_OFFSET_ALIGNMENT")]
            public static double UNIFORM_BUFFER_OFFSET_ALIGNMENT_Static
            {
                get;
            }

            [Name("ACTIVE_UNIFORM_BLOCKS")]
            public static double ACTIVE_UNIFORM_BLOCKS_Static
            {
                get;
            }

            [Name("UNIFORM_TYPE")]
            public static double UNIFORM_TYPE_Static
            {
                get;
            }

            [Name("UNIFORM_SIZE")]
            public static double UNIFORM_SIZE_Static
            {
                get;
            }

            [Name("UNIFORM_BLOCK_INDEX")]
            public static double UNIFORM_BLOCK_INDEX_Static
            {
                get;
            }

            [Name("UNIFORM_OFFSET")]
            public static double UNIFORM_OFFSET_Static
            {
                get;
            }

            [Name("UNIFORM_ARRAY_STRIDE")]
            public static double UNIFORM_ARRAY_STRIDE_Static
            {
                get;
            }

            [Name("UNIFORM_MATRIX_STRIDE")]
            public static double UNIFORM_MATRIX_STRIDE_Static
            {
                get;
            }

            [Name("UNIFORM_IS_ROW_MAJOR")]
            public static double UNIFORM_IS_ROW_MAJOR_Static
            {
                get;
            }

            [Name("UNIFORM_BLOCK_BINDING")]
            public static double UNIFORM_BLOCK_BINDING_Static
            {
                get;
            }

            [Name("UNIFORM_BLOCK_DATA_SIZE")]
            public static double UNIFORM_BLOCK_DATA_SIZE_Static
            {
                get;
            }

            [Name("UNIFORM_BLOCK_ACTIVE_UNIFORMS")]
            public static double UNIFORM_BLOCK_ACTIVE_UNIFORMS_Static
            {
                get;
            }

            [Name("UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES")]
            public static double UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES_Static
            {
                get;
            }

            [Name("UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER")]
            public static double UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER_Static
            {
                get;
            }

            [Name("UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER")]
            public static double UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER_Static
            {
                get;
            }

            [Name("INVALID_INDEX")]
            public static double INVALID_INDEX_Static
            {
                get;
            }

            [Name("MAX_VERTEX_OUTPUT_COMPONENTS")]
            public static double MAX_VERTEX_OUTPUT_COMPONENTS_Static
            {
                get;
            }

            [Name("MAX_FRAGMENT_INPUT_COMPONENTS")]
            public static double MAX_FRAGMENT_INPUT_COMPONENTS_Static
            {
                get;
            }

            [Name("MAX_SERVER_WAIT_TIMEOUT")]
            public static double MAX_SERVER_WAIT_TIMEOUT_Static
            {
                get;
            }

            [Name("OBJECT_TYPE")]
            public static double OBJECT_TYPE_Static
            {
                get;
            }

            [Name("SYNC_CONDITION")]
            public static double SYNC_CONDITION_Static
            {
                get;
            }

            [Name("SYNC_STATUS")]
            public static double SYNC_STATUS_Static
            {
                get;
            }

            [Name("SYNC_FLAGS")]
            public static double SYNC_FLAGS_Static
            {
                get;
            }

            [Name("SYNC_FENCE")]
            public static double SYNC_FENCE_Static
            {
                get;
            }

            [Name("SYNC_GPU_COMMANDS_COMPLETE")]
            public static double SYNC_GPU_COMMANDS_COMPLETE_Static
            {
                get;
            }

            [Name("UNSIGNALED")]
            public static double UNSIGNALED_Static
            {
                get;
            }

            [Name("SIGNALED")]
            public static double SIGNALED_Static
            {
                get;
            }

            [Name("ALREADY_SIGNALED")]
            public static double ALREADY_SIGNALED_Static
            {
                get;
            }

            [Name("TIMEOUT_EXPIRED")]
            public static double TIMEOUT_EXPIRED_Static
            {
                get;
            }

            [Name("CONDITION_SATISFIED")]
            public static double CONDITION_SATISFIED_Static
            {
                get;
            }

            [Name("WAIT_FAILED")]
            public static double WAIT_FAILED_Static
            {
                get;
            }

            [Name("SYNC_FLUSH_COMMANDS_BIT")]
            public static double SYNC_FLUSH_COMMANDS_BIT_Static
            {
                get;
            }

            [Name("VERTEX_ATTRIB_ARRAY_DIVISOR")]
            public static double VERTEX_ATTRIB_ARRAY_DIVISOR_Static
            {
                get;
            }

            [Name("ANY_SAMPLES_PASSED")]
            public static double ANY_SAMPLES_PASSED_Static
            {
                get;
            }

            [Name("ANY_SAMPLES_PASSED_CONSERVATIVE")]
            public static double ANY_SAMPLES_PASSED_CONSERVATIVE_Static
            {
                get;
            }

            [Name("SAMPLER_BINDING")]
            public static double SAMPLER_BINDING_Static
            {
                get;
            }

            [Name("RGB10_A2UI")]
            public static double RGB10_A2UI_Static
            {
                get;
            }

            [Name("INT_2_10_10_10_REV")]
            public static double INT_2_10_10_10_REV_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK")]
            public static double TRANSFORM_FEEDBACK_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_PAUSED")]
            public static double TRANSFORM_FEEDBACK_PAUSED_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_ACTIVE")]
            public static double TRANSFORM_FEEDBACK_ACTIVE_Static
            {
                get;
            }

            [Name("TRANSFORM_FEEDBACK_BINDING")]
            public static double TRANSFORM_FEEDBACK_BINDING_Static
            {
                get;
            }

            [Name("TEXTURE_IMMUTABLE_FORMAT")]
            public static double TEXTURE_IMMUTABLE_FORMAT_Static
            {
                get;
            }

            [Name("MAX_ELEMENT_INDEX")]
            public static double MAX_ELEMENT_INDEX_Static
            {
                get;
            }

            [Name("TEXTURE_IMMUTABLE_LEVELS")]
            public static double TEXTURE_IMMUTABLE_LEVELS_Static
            {
                get;
            }

            [Name("TIMEOUT_IGNORED")]
            public static double TIMEOUT_IGNORED_Static
            {
                get;
            }

            [Name("MAX_CLIENT_WAIT_TIMEOUT_WEBGL")]
            public static double MAX_CLIENT_WAIT_TIMEOUT_WEBGL_Static
            {
                get;
            }

            public virtual double READ_BUFFER
            {
                get;
            }

            public virtual double UNPACK_ROW_LENGTH
            {
                get;
            }

            public virtual double UNPACK_SKIP_ROWS
            {
                get;
            }

            public virtual double UNPACK_SKIP_PIXELS
            {
                get;
            }

            public virtual double PACK_ROW_LENGTH
            {
                get;
            }

            public virtual double PACK_SKIP_ROWS
            {
                get;
            }

            public virtual double PACK_SKIP_PIXELS
            {
                get;
            }

            public virtual double COLOR
            {
                get;
            }

            public virtual double DEPTH
            {
                get;
            }

            public virtual double STENCIL
            {
                get;
            }

            public virtual double RED
            {
                get;
            }

            public virtual double RGB8
            {
                get;
            }

            public virtual double RGBA8
            {
                get;
            }

            public virtual double RGB10_A2
            {
                get;
            }

            public virtual double TEXTURE_BINDING_3D
            {
                get;
            }

            public virtual double UNPACK_SKIP_IMAGES
            {
                get;
            }

            public virtual double UNPACK_IMAGE_HEIGHT
            {
                get;
            }

            public virtual double TEXTURE_3D
            {
                get;
            }

            public virtual double TEXTURE_WRAP_R
            {
                get;
            }

            public virtual double MAX_3D_TEXTURE_SIZE
            {
                get;
            }

            public virtual double UNSIGNED_INT_2_10_10_10_REV
            {
                get;
            }

            public virtual double MAX_ELEMENTS_VERTICES
            {
                get;
            }

            public virtual double MAX_ELEMENTS_INDICES
            {
                get;
            }

            public virtual double TEXTURE_MIN_LOD
            {
                get;
            }

            public virtual double TEXTURE_MAX_LOD
            {
                get;
            }

            public virtual double TEXTURE_BASE_LEVEL
            {
                get;
            }

            public virtual double TEXTURE_MAX_LEVEL
            {
                get;
            }

            public virtual double MIN
            {
                get;
            }

            public virtual double MAX
            {
                get;
            }

            public virtual double DEPTH_COMPONENT24
            {
                get;
            }

            public virtual double MAX_TEXTURE_LOD_BIAS
            {
                get;
            }

            public virtual double TEXTURE_COMPARE_MODE
            {
                get;
            }

            public virtual double TEXTURE_COMPARE_FUNC
            {
                get;
            }

            public virtual double CURRENT_QUERY
            {
                get;
            }

            public virtual double QUERY_RESULT
            {
                get;
            }

            public virtual double QUERY_RESULT_AVAILABLE
            {
                get;
            }

            public virtual double STREAM_READ
            {
                get;
            }

            public virtual double STREAM_COPY
            {
                get;
            }

            public virtual double STATIC_READ
            {
                get;
            }

            public virtual double STATIC_COPY
            {
                get;
            }

            public virtual double DYNAMIC_READ
            {
                get;
            }

            public virtual double DYNAMIC_COPY
            {
                get;
            }

            public virtual double MAX_DRAW_BUFFERS
            {
                get;
            }

            public virtual double DRAW_BUFFER0
            {
                get;
            }

            public virtual double DRAW_BUFFER1
            {
                get;
            }

            public virtual double DRAW_BUFFER2
            {
                get;
            }

            public virtual double DRAW_BUFFER3
            {
                get;
            }

            public virtual double DRAW_BUFFER4
            {
                get;
            }

            public virtual double DRAW_BUFFER5
            {
                get;
            }

            public virtual double DRAW_BUFFER6
            {
                get;
            }

            public virtual double DRAW_BUFFER7
            {
                get;
            }

            public virtual double DRAW_BUFFER8
            {
                get;
            }

            public virtual double DRAW_BUFFER9
            {
                get;
            }

            public virtual double DRAW_BUFFER10
            {
                get;
            }

            public virtual double DRAW_BUFFER11
            {
                get;
            }

            public virtual double DRAW_BUFFER12
            {
                get;
            }

            public virtual double DRAW_BUFFER13
            {
                get;
            }

            public virtual double DRAW_BUFFER14
            {
                get;
            }

            public virtual double DRAW_BUFFER15
            {
                get;
            }

            public virtual double MAX_FRAGMENT_UNIFORM_COMPONENTS
            {
                get;
            }

            public virtual double MAX_VERTEX_UNIFORM_COMPONENTS
            {
                get;
            }

            public virtual double SAMPLER_3D
            {
                get;
            }

            public virtual double SAMPLER_2D_SHADOW
            {
                get;
            }

            public virtual double FRAGMENT_SHADER_DERIVATIVE_HINT
            {
                get;
            }

            public virtual double PIXEL_PACK_BUFFER
            {
                get;
            }

            public virtual double PIXEL_UNPACK_BUFFER
            {
                get;
            }

            public virtual double PIXEL_PACK_BUFFER_BINDING
            {
                get;
            }

            public virtual double PIXEL_UNPACK_BUFFER_BINDING
            {
                get;
            }

            public virtual double FLOAT_MAT2x3
            {
                get;
            }

            public virtual double FLOAT_MAT2x4
            {
                get;
            }

            public virtual double FLOAT_MAT3x2
            {
                get;
            }

            public virtual double FLOAT_MAT3x4
            {
                get;
            }

            public virtual double FLOAT_MAT4x2
            {
                get;
            }

            public virtual double FLOAT_MAT4x3
            {
                get;
            }

            public virtual double SRGB
            {
                get;
            }

            public virtual double SRGB8
            {
                get;
            }

            public virtual double SRGB8_ALPHA8
            {
                get;
            }

            public virtual double COMPARE_REF_TO_TEXTURE
            {
                get;
            }

            public virtual double RGBA32F
            {
                get;
            }

            public virtual double RGB32F
            {
                get;
            }

            public virtual double RGBA16F
            {
                get;
            }

            public virtual double RGB16F
            {
                get;
            }

            public virtual double VERTEX_ATTRIB_ARRAY_INTEGER
            {
                get;
            }

            public virtual double MAX_ARRAY_TEXTURE_LAYERS
            {
                get;
            }

            public virtual double MIN_PROGRAM_TEXEL_OFFSET
            {
                get;
            }

            public virtual double MAX_PROGRAM_TEXEL_OFFSET
            {
                get;
            }

            public virtual double MAX_VARYING_COMPONENTS
            {
                get;
            }

            public virtual double TEXTURE_2D_ARRAY
            {
                get;
            }

            public virtual double TEXTURE_BINDING_2D_ARRAY
            {
                get;
            }

            public virtual double R11F_G11F_B10F
            {
                get;
            }

            public virtual double UNSIGNED_INT_10F_11F_11F_REV
            {
                get;
            }

            public virtual double RGB9_E5
            {
                get;
            }

            public virtual double UNSIGNED_INT_5_9_9_9_REV
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_BUFFER_MODE
            {
                get;
            }

            public virtual double MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_VARYINGS
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_BUFFER_START
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_BUFFER_SIZE
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN
            {
                get;
            }

            public virtual double RASTERIZER_DISCARD
            {
                get;
            }

            public virtual double MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS
            {
                get;
            }

            public virtual double MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS
            {
                get;
            }

            public virtual double INTERLEAVED_ATTRIBS
            {
                get;
            }

            public virtual double SEPARATE_ATTRIBS
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_BUFFER
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_BUFFER_BINDING
            {
                get;
            }

            public virtual double RGBA32UI
            {
                get;
            }

            public virtual double RGB32UI
            {
                get;
            }

            public virtual double RGBA16UI
            {
                get;
            }

            public virtual double RGB16UI
            {
                get;
            }

            public virtual double RGBA8UI
            {
                get;
            }

            public virtual double RGB8UI
            {
                get;
            }

            public virtual double RGBA32I
            {
                get;
            }

            public virtual double RGB32I
            {
                get;
            }

            public virtual double RGBA16I
            {
                get;
            }

            public virtual double RGB16I
            {
                get;
            }

            public virtual double RGBA8I
            {
                get;
            }

            public virtual double RGB8I
            {
                get;
            }

            public virtual double RED_INTEGER
            {
                get;
            }

            public virtual double RGB_INTEGER
            {
                get;
            }

            public virtual double RGBA_INTEGER
            {
                get;
            }

            public virtual double SAMPLER_2D_ARRAY
            {
                get;
            }

            public virtual double SAMPLER_2D_ARRAY_SHADOW
            {
                get;
            }

            public virtual double SAMPLER_CUBE_SHADOW
            {
                get;
            }

            public virtual double UNSIGNED_INT_VEC2
            {
                get;
            }

            public virtual double UNSIGNED_INT_VEC3
            {
                get;
            }

            public virtual double UNSIGNED_INT_VEC4
            {
                get;
            }

            public virtual double INT_SAMPLER_2D
            {
                get;
            }

            public virtual double INT_SAMPLER_3D
            {
                get;
            }

            public virtual double INT_SAMPLER_CUBE
            {
                get;
            }

            public virtual double INT_SAMPLER_2D_ARRAY
            {
                get;
            }

            public virtual double UNSIGNED_INT_SAMPLER_2D
            {
                get;
            }

            public virtual double UNSIGNED_INT_SAMPLER_3D
            {
                get;
            }

            public virtual double UNSIGNED_INT_SAMPLER_CUBE
            {
                get;
            }

            public virtual double UNSIGNED_INT_SAMPLER_2D_ARRAY
            {
                get;
            }

            public virtual double DEPTH_COMPONENT32F
            {
                get;
            }

            public virtual double DEPTH32F_STENCIL8
            {
                get;
            }

            public virtual double FLOAT_32_UNSIGNED_INT_24_8_REV
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_RED_SIZE
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_GREEN_SIZE
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_BLUE_SIZE
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE
            {
                get;
            }

            public virtual double FRAMEBUFFER_DEFAULT
            {
                get;
            }

            public virtual double UNSIGNED_INT_24_8
            {
                get;
            }

            public virtual double DEPTH24_STENCIL8
            {
                get;
            }

            public virtual double UNSIGNED_NORMALIZED
            {
                get;
            }

            public virtual double DRAW_FRAMEBUFFER_BINDING
            {
                get;
            }

            public virtual double READ_FRAMEBUFFER
            {
                get;
            }

            public virtual double DRAW_FRAMEBUFFER
            {
                get;
            }

            public virtual double READ_FRAMEBUFFER_BINDING
            {
                get;
            }

            public virtual double RENDERBUFFER_SAMPLES
            {
                get;
            }

            public virtual double FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER
            {
                get;
            }

            public virtual double MAX_COLOR_ATTACHMENTS
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT1
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT2
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT3
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT4
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT5
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT6
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT7
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT8
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT9
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT10
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT11
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT12
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT13
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT14
            {
                get;
            }

            public virtual double COLOR_ATTACHMENT15
            {
                get;
            }

            public virtual double FRAMEBUFFER_INCOMPLETE_MULTISAMPLE
            {
                get;
            }

            public virtual double MAX_SAMPLES
            {
                get;
            }

            public virtual double HALF_FLOAT
            {
                get;
            }

            public virtual double RG
            {
                get;
            }

            public virtual double RG_INTEGER
            {
                get;
            }

            public virtual double R8
            {
                get;
            }

            public virtual double RG8
            {
                get;
            }

            public virtual double R16F
            {
                get;
            }

            public virtual double R32F
            {
                get;
            }

            public virtual double RG16F
            {
                get;
            }

            public virtual double RG32F
            {
                get;
            }

            public virtual double R8I
            {
                get;
            }

            public virtual double R8UI
            {
                get;
            }

            public virtual double R16I
            {
                get;
            }

            public virtual double R16UI
            {
                get;
            }

            public virtual double R32I
            {
                get;
            }

            public virtual double R32UI
            {
                get;
            }

            public virtual double RG8I
            {
                get;
            }

            public virtual double RG8UI
            {
                get;
            }

            public virtual double RG16I
            {
                get;
            }

            public virtual double RG16UI
            {
                get;
            }

            public virtual double RG32I
            {
                get;
            }

            public virtual double RG32UI
            {
                get;
            }

            public virtual double VERTEX_ARRAY_BINDING
            {
                get;
            }

            public virtual double R8_SNORM
            {
                get;
            }

            public virtual double RG8_SNORM
            {
                get;
            }

            public virtual double RGB8_SNORM
            {
                get;
            }

            public virtual double RGBA8_SNORM
            {
                get;
            }

            public virtual double SIGNED_NORMALIZED
            {
                get;
            }

            public virtual double COPY_READ_BUFFER
            {
                get;
            }

            public virtual double COPY_WRITE_BUFFER
            {
                get;
            }

            public virtual double COPY_READ_BUFFER_BINDING
            {
                get;
            }

            public virtual double COPY_WRITE_BUFFER_BINDING
            {
                get;
            }

            public virtual double UNIFORM_BUFFER
            {
                get;
            }

            public virtual double UNIFORM_BUFFER_BINDING
            {
                get;
            }

            public virtual double UNIFORM_BUFFER_START
            {
                get;
            }

            public virtual double UNIFORM_BUFFER_SIZE
            {
                get;
            }

            public virtual double MAX_VERTEX_UNIFORM_BLOCKS
            {
                get;
            }

            public virtual double MAX_FRAGMENT_UNIFORM_BLOCKS
            {
                get;
            }

            public virtual double MAX_COMBINED_UNIFORM_BLOCKS
            {
                get;
            }

            public virtual double MAX_UNIFORM_BUFFER_BINDINGS
            {
                get;
            }

            public virtual double MAX_UNIFORM_BLOCK_SIZE
            {
                get;
            }

            public virtual double MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS
            {
                get;
            }

            public virtual double MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS
            {
                get;
            }

            public virtual double UNIFORM_BUFFER_OFFSET_ALIGNMENT
            {
                get;
            }

            public virtual double ACTIVE_UNIFORM_BLOCKS
            {
                get;
            }

            public virtual double UNIFORM_TYPE
            {
                get;
            }

            public virtual double UNIFORM_SIZE
            {
                get;
            }

            public virtual double UNIFORM_BLOCK_INDEX
            {
                get;
            }

            public virtual double UNIFORM_OFFSET
            {
                get;
            }

            public virtual double UNIFORM_ARRAY_STRIDE
            {
                get;
            }

            public virtual double UNIFORM_MATRIX_STRIDE
            {
                get;
            }

            public virtual double UNIFORM_IS_ROW_MAJOR
            {
                get;
            }

            public virtual double UNIFORM_BLOCK_BINDING
            {
                get;
            }

            public virtual double UNIFORM_BLOCK_DATA_SIZE
            {
                get;
            }

            public virtual double UNIFORM_BLOCK_ACTIVE_UNIFORMS
            {
                get;
            }

            public virtual double UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES
            {
                get;
            }

            public virtual double UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER
            {
                get;
            }

            public virtual double UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER
            {
                get;
            }

            public virtual double INVALID_INDEX
            {
                get;
            }

            public virtual double MAX_VERTEX_OUTPUT_COMPONENTS
            {
                get;
            }

            public virtual double MAX_FRAGMENT_INPUT_COMPONENTS
            {
                get;
            }

            public virtual double MAX_SERVER_WAIT_TIMEOUT
            {
                get;
            }

            public virtual double OBJECT_TYPE
            {
                get;
            }

            public virtual double SYNC_CONDITION
            {
                get;
            }

            public virtual double SYNC_STATUS
            {
                get;
            }

            public virtual double SYNC_FLAGS
            {
                get;
            }

            public virtual double SYNC_FENCE
            {
                get;
            }

            public virtual double SYNC_GPU_COMMANDS_COMPLETE
            {
                get;
            }

            public virtual double UNSIGNALED
            {
                get;
            }

            public virtual double SIGNALED
            {
                get;
            }

            public virtual double ALREADY_SIGNALED
            {
                get;
            }

            public virtual double TIMEOUT_EXPIRED
            {
                get;
            }

            public virtual double CONDITION_SATISFIED
            {
                get;
            }

            public virtual double WAIT_FAILED
            {
                get;
            }

            public virtual double SYNC_FLUSH_COMMANDS_BIT
            {
                get;
            }

            public virtual double VERTEX_ATTRIB_ARRAY_DIVISOR
            {
                get;
            }

            public virtual double ANY_SAMPLES_PASSED
            {
                get;
            }

            public virtual double ANY_SAMPLES_PASSED_CONSERVATIVE
            {
                get;
            }

            public virtual double SAMPLER_BINDING
            {
                get;
            }

            public virtual double RGB10_A2UI
            {
                get;
            }

            public virtual double INT_2_10_10_10_REV
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_PAUSED
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_ACTIVE
            {
                get;
            }

            public virtual double TRANSFORM_FEEDBACK_BINDING
            {
                get;
            }

            public virtual double TEXTURE_IMMUTABLE_FORMAT
            {
                get;
            }

            public virtual double MAX_ELEMENT_INDEX
            {
                get;
            }

            public virtual double TEXTURE_IMMUTABLE_LEVELS
            {
                get;
            }

            public virtual double TIMEOUT_IGNORED
            {
                get;
            }

            public virtual double MAX_CLIENT_WAIT_TIMEOUT_WEBGL
            {
                get;
            }

            public virtual extern void bufferData(
              double target,
              Union<es5.ArrayBuffer, es5.ArrayBufferView, Null> srcData,
              double usage);

            public virtual extern void bufferData(
              double target,
              es5.ArrayBufferView srcData,
              double usage);

            public virtual extern void bufferSubData(
              double target,
              double dstByteOffset,
              Union<es5.ArrayBuffer, es5.ArrayBufferView, Null> srcData);

            public virtual extern void bufferSubData(
              double target,
              double dstByteOffset,
              es5.ArrayBufferView srcData);

            public virtual extern void bufferData(
              double target,
              es5.ArrayBufferView srcData,
              double usage,
              double srcOffset);

            public virtual extern void bufferData(
              double target,
              es5.ArrayBufferView srcData,
              double usage,
              double srcOffset,
              double length);

            public virtual extern void bufferSubData(
              double target,
              double dstByteOffset,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void bufferSubData(
              double target,
              double dstByteOffset,
              es5.ArrayBufferView srcData,
              double srcOffset,
              double length);

            public virtual extern void copyBufferSubData(
              double readTarget,
              double writeTarget,
              double readOffset,
              double writeOffset,
              double size);

            public virtual extern void getBufferSubData(
              double target,
              double srcByteOffset,
              es5.ArrayBufferView dstBuffer);

            public virtual extern void getBufferSubData(
              double target,
              double srcByteOffset,
              es5.ArrayBufferView dstBuffer,
              double dstOffset);

            public virtual extern void getBufferSubData(
              double target,
              double srcByteOffset,
              es5.ArrayBufferView dstBuffer,
              double dstOffset,
              double length);

            public virtual extern void blitFramebuffer(
              double srcX0,
              double srcY0,
              double srcX1,
              double srcY1,
              double dstX0,
              double dstY0,
              double dstX1,
              double dstY1,
              double mask,
              double filter);

            public virtual extern void framebufferTextureLayer(
              double target,
              double attachment,
              dom.WebGLTexture texture,
              double level,
              double layer);

            public virtual extern void invalidateFramebuffer(double target, double[] attachments);

            public virtual extern void invalidateSubFramebuffer(
              double target,
              double[] attachments,
              double x,
              double y,
              double width,
              double height);

            public virtual extern void readBuffer(double src);

            public virtual extern object getInternalformatParameter(
              double target,
              double internalformat,
              double pname);

            public virtual extern void renderbufferStorageMultisample(
              double target,
              double samples,
              double internalformat,
              double width,
              double height);

            public virtual extern void texStorage2D(
              double target,
              double levels,
              double internalformat,
              double width,
              double height);

            public virtual extern void texStorage3D(
              double target,
              double levels,
              double internalformat,
              double width,
              double height,
              double depth);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double format,
              double type,
              Union<dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double format,
              double type,
              webgl2.HTMLCanvasElement source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double format,
              double type,
              Union<webgl2.ImageBitmap, dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double format,
              double type,
              webgl2.ImageBitmap source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double format,
              double type,
              Union<dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double format,
              double type,
              webgl2.HTMLCanvasElement source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double format,
              double type,
              Union<webgl2.ImageBitmap, dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double format,
              double type,
              webgl2.ImageBitmap source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              double pboOffset);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              Union<webgl2.ImageBitmap, dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              webgl2.ImageBitmap source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              dom.ImageData source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              dom.HTMLImageElement source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              webgl2.HTMLCanvasElement source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              dom.HTMLVideoElement source);

            public virtual extern void texImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double format,
              double type,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              double pboOffset);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              Union<webgl2.ImageBitmap, dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              webgl2.ImageBitmap source);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              dom.ImageData source);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              dom.HTMLImageElement source);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              webgl2.HTMLCanvasElement source);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              dom.HTMLVideoElement source);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              es5.ArrayBufferView srcData);

            public virtual extern void texImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double format,
              double type,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              double pboOffset);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              Union<webgl2.ImageBitmap, dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              webgl2.ImageBitmap source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              dom.ImageData source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              dom.HTMLImageElement source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              webgl2.HTMLCanvasElement source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              dom.HTMLVideoElement source);

            public virtual extern void texSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double type,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              double pboOffset);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              Union<webgl2.ImageBitmap, dom.ImageData, dom.HTMLImageElement, webgl2.HTMLCanvasElement, dom.HTMLVideoElement> source);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              webgl2.ImageBitmap source);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              dom.ImageData source);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              dom.HTMLImageElement source);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              webgl2.HTMLCanvasElement source);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              dom.HTMLVideoElement source);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              es5.ArrayBufferView srcData);

            public virtual extern void texSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double type,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void copyTexSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double x,
              double y,
              double width,
              double height);

            public virtual extern void compressedTexImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              double imageSize,
              double offset);

            public virtual extern void compressedTexImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              es5.ArrayBufferView srcData);

            public virtual extern void compressedTexImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void compressedTexImage2D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double border,
              es5.ArrayBufferView srcData,
              double srcOffset,
              double srcLengthOverride);

            public virtual extern void compressedTexImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              double imageSize,
              double offset);

            public virtual extern void compressedTexImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              es5.ArrayBufferView srcData);

            public virtual extern void compressedTexImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void compressedTexImage3D(
              double target,
              double level,
              double internalformat,
              double width,
              double height,
              double depth,
              double border,
              es5.ArrayBufferView srcData,
              double srcOffset,
              double srcLengthOverride);

            public virtual extern void compressedTexSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              double imageSize,
              double offset);

            public virtual extern void compressedTexSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              es5.ArrayBufferView srcData);

            public virtual extern void compressedTexSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void compressedTexSubImage2D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double width,
              double height,
              double format,
              es5.ArrayBufferView srcData,
              double srcOffset,
              double srcLengthOverride);

            public virtual extern void compressedTexSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              double imageSize,
              double offset);

            public virtual extern void compressedTexSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              es5.ArrayBufferView srcData);

            public virtual extern void compressedTexSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              es5.ArrayBufferView srcData,
              double srcOffset);

            public virtual extern void compressedTexSubImage3D(
              double target,
              double level,
              double xoffset,
              double yoffset,
              double zoffset,
              double width,
              double height,
              double depth,
              double format,
              es5.ArrayBufferView srcData,
              double srcOffset,
              double srcLengthOverride);

            public virtual extern double getFragDataLocation(dom.WebGLProgram program, string name);

            public virtual extern void uniform1ui(dom.WebGLUniformLocation location, double v0);

            public virtual extern void uniform2ui(
              dom.WebGLUniformLocation location,
              double v0,
              double v1);

            public virtual extern void uniform3ui(
              dom.WebGLUniformLocation location,
              double v0,
              double v1,
              double v2);

            public virtual extern void uniform4ui(
              dom.WebGLUniformLocation location,
              double v0,
              double v1,
              double v2,
              double v3);

            public virtual extern void uniform1fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniform1fv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform1fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform1fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniform1fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform1fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniform2fv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform2fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform2fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniform2fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform2fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniform3fv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform3fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform3fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniform3fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform3fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniform4fv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform4fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform4fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniform4fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform4fv(
              dom.WebGLUniformLocation location,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4fv(
              dom.WebGLUniformLocation location,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4fv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data);

            public virtual extern void uniform1iv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform1iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform1iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset);

            public virtual extern void uniform1iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform1iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data);

            public virtual extern void uniform2iv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform2iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform2iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset);

            public virtual extern void uniform2iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform2iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data);

            public virtual extern void uniform3iv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform3iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform3iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset);

            public virtual extern void uniform3iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform3iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data);

            public virtual extern void uniform4iv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform4iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform4iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset);

            public virtual extern void uniform4iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform4iv(
              dom.WebGLUniformLocation location,
              Union<es5.Int32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4iv(
              dom.WebGLUniformLocation location,
              es5.Int32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4iv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data);

            public virtual extern void uniform1uiv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform1uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data);

            public virtual extern void uniform2uiv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform2uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data);

            public virtual extern void uniform3uiv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform3uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data);

            public virtual extern void uniform4uiv(dom.WebGLUniformLocation location, double[] data);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              Union<es5.Uint32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              es5.Uint32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniform4uiv(
              dom.WebGLUniformLocation location,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4x2fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4x3fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix2x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix3x4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              Union<es5.Float32Array, double[]> data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              es5.Float32Array data,
              double srcOffset,
              double srcLength);

            public virtual extern void uniformMatrix4fv(
              dom.WebGLUniformLocation location,
              bool transpose,
              double[] data,
              double srcOffset,
              double srcLength);

            public virtual extern void vertexAttribI4i(
              double index,
              double x,
              double y,
              double z,
              double w);

            public virtual extern void vertexAttribI4iv(
              double index,
              Union<es5.Int32Array, double[]> values);

            public virtual extern void vertexAttribI4iv(double index, es5.Int32Array values);

            public virtual extern void vertexAttribI4iv(double index, double[] values);

            public virtual extern void vertexAttribI4ui(
              double index,
              double x,
              double y,
              double z,
              double w);

            public virtual extern void vertexAttribI4uiv(
              double index,
              Union<es5.Uint32Array, double[]> values);

            public virtual extern void vertexAttribI4uiv(double index, es5.Uint32Array values);

            public virtual extern void vertexAttribI4uiv(double index, double[] values);

            public virtual extern void vertexAttribIPointer(
              double index,
              double size,
              double type,
              double stride,
              double offset);

            public virtual extern void vertexAttribDivisor(double index, double divisor);

            public virtual extern void drawArraysInstanced(
              double mode,
              double first,
              double count,
              double instanceCount);

            public virtual extern void drawElementsInstanced(
              double mode,
              double count,
              double type,
              double offset,
              double instanceCount);

            public virtual extern void drawRangeElements(
              double mode,
              double start,
              double end,
              double count,
              double type,
              double offset);

            public virtual extern void readPixels(
              double x,
              double y,
              double width,
              double height,
              double format,
              double type,
              es5.ArrayBufferView dstData);

            public virtual extern void readPixels(
              double x,
              double y,
              double width,
              double height,
              double format,
              double type,
              double offset);

            public virtual extern void readPixels(
              double x,
              double y,
              double width,
              double height,
              double format,
              double type,
              es5.ArrayBufferView dstData,
              double dstOffset);

            public virtual extern void drawBuffers(double[] buffers);

            public virtual extern void clearBufferfv(
              double buffer,
              double drawbuffer,
              Union<es5.Float32Array, double[]> values);

            public virtual extern void clearBufferfv(
              double buffer,
              double drawbuffer,
              es5.Float32Array values);

            public virtual extern void clearBufferfv(double buffer, double drawbuffer, double[] values);

            public virtual extern void clearBufferfv(
              double buffer,
              double drawbuffer,
              Union<es5.Float32Array, double[]> values,
              double srcOffset);

            public virtual extern void clearBufferfv(
              double buffer,
              double drawbuffer,
              es5.Float32Array values,
              double srcOffset);

            public virtual extern void clearBufferfv(
              double buffer,
              double drawbuffer,
              double[] values,
              double srcOffset);

            public virtual extern void clearBufferiv(
              double buffer,
              double drawbuffer,
              Union<es5.Int32Array, double[]> values);

            public virtual extern void clearBufferiv(
              double buffer,
              double drawbuffer,
              es5.Int32Array values);

            public virtual extern void clearBufferiv(double buffer, double drawbuffer, double[] values);

            public virtual extern void clearBufferiv(
              double buffer,
              double drawbuffer,
              Union<es5.Int32Array, double[]> values,
              double srcOffset);

            public virtual extern void clearBufferiv(
              double buffer,
              double drawbuffer,
              es5.Int32Array values,
              double srcOffset);

            public virtual extern void clearBufferiv(
              double buffer,
              double drawbuffer,
              double[] values,
              double srcOffset);

            public virtual extern void clearBufferuiv(
              double buffer,
              double drawbuffer,
              Union<es5.Uint32Array, double[]> values);

            public virtual extern void clearBufferuiv(
              double buffer,
              double drawbuffer,
              es5.Uint32Array values);

            public virtual extern void clearBufferuiv(double buffer, double drawbuffer, double[] values);

            public virtual extern void clearBufferuiv(
              double buffer,
              double drawbuffer,
              Union<es5.Uint32Array, double[]> values,
              double srcOffset);

            public virtual extern void clearBufferuiv(
              double buffer,
              double drawbuffer,
              es5.Uint32Array values,
              double srcOffset);

            public virtual extern void clearBufferuiv(
              double buffer,
              double drawbuffer,
              double[] values,
              double srcOffset);

            public virtual extern void clearBufferfi(
              double buffer,
              double drawbuffer,
              double depth,
              double stencil);

            public virtual extern webgl2.WebGLQuery createQuery();

            public virtual extern void deleteQuery(webgl2.WebGLQuery query);

            public virtual extern bool isQuery(webgl2.WebGLQuery query);

            public virtual extern void beginQuery(double target, webgl2.WebGLQuery query);

            public virtual extern void endQuery(double target);

            public virtual extern webgl2.WebGLQuery getQuery(double target, double pname);

            public virtual extern object getQueryParameter(webgl2.WebGLQuery query, double pname);

            public virtual extern webgl2.WebGLSampler createSampler();

            public virtual extern void deleteSampler(webgl2.WebGLSampler sampler);

            public virtual extern bool isSampler(webgl2.WebGLSampler sampler);

            public virtual extern void bindSampler(double unit, webgl2.WebGLSampler sampler);

            public virtual extern void samplerParameteri(
              webgl2.WebGLSampler sampler,
              double pname,
              double param);

            public virtual extern void samplerParameterf(
              webgl2.WebGLSampler sampler,
              double pname,
              double param);

            public virtual extern object getSamplerParameter(webgl2.WebGLSampler sampler, double pname);

            public virtual extern webgl2.WebGLSync fenceSync(double condition, double flags);

            public virtual extern bool isSync(webgl2.WebGLSync sync);

            public virtual extern void deleteSync(webgl2.WebGLSync sync);

            public virtual extern double clientWaitSync(
              webgl2.WebGLSync sync,
              double flags,
              double timeout);

            public virtual extern void waitSync(webgl2.WebGLSync sync, double flags, double timeout);

            public virtual extern object getSyncParameter(webgl2.WebGLSync sync, double pname);

            public virtual extern webgl2.WebGLTransformFeedback createTransformFeedback();

            public virtual extern void deleteTransformFeedback(webgl2.WebGLTransformFeedback tf);

            public virtual extern bool isTransformFeedback(webgl2.WebGLTransformFeedback tf);

            public virtual extern void bindTransformFeedback(
              double target,
              webgl2.WebGLTransformFeedback tf);

            public virtual extern void beginTransformFeedback(double primitiveMode);

            public virtual extern void endTransformFeedback();

            public virtual extern void transformFeedbackVaryings(
              dom.WebGLProgram program,
              string[] varyings,
              double bufferMode);

            public virtual extern dom.WebGLActiveInfo getTransformFeedbackVarying(
              dom.WebGLProgram program,
              double index);

            public virtual extern void pauseTransformFeedback();

            public virtual extern void resumeTransformFeedback();

            public virtual extern void bindBufferBase(
              double target,
              double index,
              dom.WebGLBuffer buffer);

            public virtual extern void bindBufferRange(
              double target,
              double index,
              dom.WebGLBuffer buffer,
              double offset,
              double size);

            public virtual extern object getIndexedParameter(double target, double index);

            public virtual extern double[] getUniformIndices(
              dom.WebGLProgram program,
              string[] uniformNames);

            public virtual extern object getActiveUniforms(
              dom.WebGLProgram program,
              double[] uniformIndices,
              double pname);

            public virtual extern double getUniformBlockIndex(
              dom.WebGLProgram program,
              string uniformBlockName);

            public virtual extern object getActiveUniformBlockParameter(
              dom.WebGLProgram program,
              double uniformBlockIndex,
              double pname);

            public virtual extern string getActiveUniformBlockName(
              dom.WebGLProgram program,
              double uniformBlockIndex);

            public virtual extern void uniformBlockBinding(
              dom.WebGLProgram program,
              double uniformBlockIndex,
              double uniformBlockBinding);

            public virtual extern webgl2.WebGLVertexArrayObject createVertexArray();

            public virtual extern void deleteVertexArray(webgl2.WebGLVertexArrayObject vertexArray);

            public virtual extern bool isVertexArray(webgl2.WebGLVertexArrayObject vertexArray);

            public virtual extern void bindVertexArray(webgl2.WebGLVertexArrayObject array);
        }

        [CombinedClass]
        [FormerInterface]
        public class WebGLQuery : dom.WebGLObject
        {

            public static webgl2.WebGLQuery prototype
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class WebGLSampler : dom.WebGLObject
        {

            public static webgl2.WebGLSampler prototype
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class WebGLSync : dom.WebGLObject
        {

            public static webgl2.WebGLSync prototype
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class WebGLTransformFeedback : dom.WebGLObject
        {

            public static webgl2.WebGLTransformFeedback prototype
            {
                get;
                set;
            }
        }

        [CombinedClass]
        [FormerInterface]
        public class WebGLVertexArrayObject : dom.WebGLObject
        {

            public static webgl2.WebGLVertexArrayObject prototype
            {
                get;
                set;
            }
        }

        [Virtual]
        public abstract class WebGL2RenderingContextTypeConfig : IObject
        {

            public virtual webgl2.WebGL2RenderingContext prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract webgl2.WebGL2RenderingContext New();

            public abstract double ACTIVE_ATTRIBUTES { get; }

            public abstract double ACTIVE_TEXTURE { get; }

            public abstract double ACTIVE_UNIFORMS { get; }

            public abstract double ALIASED_LINE_WIDTH_RANGE { get; }

            public abstract double ALIASED_POINT_SIZE_RANGE { get; }

            public abstract double ALPHA { get; }

            public abstract double ALPHA_BITS { get; }

            public abstract double ALWAYS { get; }

            public abstract double ARRAY_BUFFER { get; }

            public abstract double ARRAY_BUFFER_BINDING { get; }

            public abstract double ATTACHED_SHADERS { get; }

            public abstract double BACK { get; }

            public abstract double BLEND { get; }

            public abstract double BLEND_COLOR { get; }

            public abstract double BLEND_DST_ALPHA { get; }

            public abstract double BLEND_DST_RGB { get; }

            public abstract double BLEND_EQUATION { get; }

            public abstract double BLEND_EQUATION_ALPHA { get; }

            public abstract double BLEND_EQUATION_RGB { get; }

            public abstract double BLEND_SRC_ALPHA { get; }

            public abstract double BLEND_SRC_RGB { get; }

            public abstract double BLUE_BITS { get; }

            public abstract double BOOL { get; }

            public abstract double BOOL_VEC2 { get; }

            public abstract double BOOL_VEC3 { get; }

            public abstract double BOOL_VEC4 { get; }

            public abstract double BROWSER_DEFAULT_WEBGL { get; }

            public abstract double BUFFER_SIZE { get; }

            public abstract double BUFFER_USAGE { get; }

            public abstract double BYTE { get; }

            public abstract double CCW { get; }

            public abstract double CLAMP_TO_EDGE { get; }

            public abstract double COLOR_ATTACHMENT0 { get; }

            public abstract double COLOR_BUFFER_BIT { get; }

            public abstract double COLOR_CLEAR_VALUE { get; }

            public abstract double COLOR_WRITEMASK { get; }

            public abstract double COMPILE_STATUS { get; }

            public abstract double COMPRESSED_TEXTURE_FORMATS { get; }

            public abstract double CONSTANT_ALPHA { get; }

            public abstract double CONSTANT_COLOR { get; }

            public abstract double CONTEXT_LOST_WEBGL { get; }

            public abstract double CULL_FACE { get; }

            public abstract double CULL_FACE_MODE { get; }

            public abstract double CURRENT_PROGRAM { get; }

            public abstract double CURRENT_VERTEX_ATTRIB { get; }

            public abstract double CW { get; }

            public abstract double DECR { get; }

            public abstract double DECR_WRAP { get; }

            public abstract double DELETE_STATUS { get; }

            public abstract double DEPTH_ATTACHMENT { get; }

            public abstract double DEPTH_BITS { get; }

            public abstract double DEPTH_BUFFER_BIT { get; }

            public abstract double DEPTH_CLEAR_VALUE { get; }

            public abstract double DEPTH_COMPONENT { get; }

            public abstract double DEPTH_COMPONENT16 { get; }

            public abstract double DEPTH_FUNC { get; }

            public abstract double DEPTH_RANGE { get; }

            public abstract double DEPTH_STENCIL { get; }

            public abstract double DEPTH_STENCIL_ATTACHMENT { get; }

            public abstract double DEPTH_TEST { get; }

            public abstract double DEPTH_WRITEMASK { get; }

            public abstract double DITHER { get; }

            public abstract double DONT_CARE { get; }

            public abstract double DST_ALPHA { get; }

            public abstract double DST_COLOR { get; }

            public abstract double DYNAMIC_DRAW { get; }

            public abstract double ELEMENT_ARRAY_BUFFER { get; }

            public abstract double ELEMENT_ARRAY_BUFFER_BINDING { get; }

            public abstract double EQUAL { get; }

            public abstract double FASTEST { get; }

            public abstract double FLOAT { get; }

            public abstract double FLOAT_MAT2 { get; }

            public abstract double FLOAT_MAT3 { get; }

            public abstract double FLOAT_MAT4 { get; }

            public abstract double FLOAT_VEC2 { get; }

            public abstract double FLOAT_VEC3 { get; }

            public abstract double FLOAT_VEC4 { get; }

            public abstract double FRAGMENT_SHADER { get; }

            public abstract double FRAMEBUFFER { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_OBJECT_NAME { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL { get; }

            public abstract double FRAMEBUFFER_BINDING { get; }

            public abstract double FRAMEBUFFER_COMPLETE { get; }

            public abstract double FRAMEBUFFER_INCOMPLETE_ATTACHMENT { get; }

            public abstract double FRAMEBUFFER_INCOMPLETE_DIMENSIONS { get; }

            public abstract double FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT { get; }

            public abstract double FRAMEBUFFER_UNSUPPORTED { get; }

            public abstract double FRONT { get; }

            public abstract double FRONT_AND_BACK { get; }

            public abstract double FRONT_FACE { get; }

            public abstract double FUNC_ADD { get; }

            public abstract double FUNC_REVERSE_SUBTRACT { get; }

            public abstract double FUNC_SUBTRACT { get; }

            public abstract double GENERATE_MIPMAP_HINT { get; }

            public abstract double GEQUAL { get; }

            public abstract double GREATER { get; }

            public abstract double GREEN_BITS { get; }

            public abstract double HIGH_FLOAT { get; }

            public abstract double HIGH_INT { get; }

            public abstract double IMPLEMENTATION_COLOR_READ_FORMAT { get; }

            public abstract double IMPLEMENTATION_COLOR_READ_TYPE { get; }

            public abstract double INCR { get; }

            public abstract double INCR_WRAP { get; }

            public abstract double INT { get; }

            public abstract double INT_VEC2 { get; }

            public abstract double INT_VEC3 { get; }

            public abstract double INT_VEC4 { get; }

            public abstract double INVALID_ENUM { get; }

            public abstract double INVALID_FRAMEBUFFER_OPERATION { get; }

            public abstract double INVALID_OPERATION { get; }

            public abstract double INVALID_VALUE { get; }

            public abstract double INVERT { get; }

            public abstract double KEEP { get; }

            public abstract double LEQUAL { get; }

            public abstract double LESS { get; }

            public abstract double LINEAR { get; }

            public abstract double LINEAR_MIPMAP_LINEAR { get; }

            public abstract double LINEAR_MIPMAP_NEAREST { get; }

            public abstract double LINES { get; }

            public abstract double LINE_LOOP { get; }

            public abstract double LINE_STRIP { get; }

            public abstract double LINE_WIDTH { get; }

            public abstract double LINK_STATUS { get; }

            public abstract double LOW_FLOAT { get; }

            public abstract double LOW_INT { get; }

            public abstract double LUMINANCE { get; }

            public abstract double LUMINANCE_ALPHA { get; }

            public abstract double MAX_COMBINED_TEXTURE_IMAGE_UNITS { get; }

            public abstract double MAX_CUBE_MAP_TEXTURE_SIZE { get; }

            public abstract double MAX_FRAGMENT_UNIFORM_VECTORS { get; }

            public abstract double MAX_RENDERBUFFER_SIZE { get; }

            public abstract double MAX_TEXTURE_IMAGE_UNITS { get; }

            public abstract double MAX_TEXTURE_SIZE { get; }

            public abstract double MAX_VARYING_VECTORS { get; }

            public abstract double MAX_VERTEX_ATTRIBS { get; }

            public abstract double MAX_VERTEX_TEXTURE_IMAGE_UNITS { get; }

            public abstract double MAX_VERTEX_UNIFORM_VECTORS { get; }

            public abstract double MAX_VIEWPORT_DIMS { get; }

            public abstract double MEDIUM_FLOAT { get; }

            public abstract double MEDIUM_INT { get; }

            public abstract double MIRRORED_REPEAT { get; }

            public abstract double NEAREST { get; }

            public abstract double NEAREST_MIPMAP_LINEAR { get; }

            public abstract double NEAREST_MIPMAP_NEAREST { get; }

            public abstract double NEVER { get; }

            public abstract double NICEST { get; }

            public abstract double NONE { get; }

            public abstract double NOTEQUAL { get; }

            public abstract double NO_ERROR { get; }

            public abstract double ONE { get; }

            public abstract double ONE_MINUS_CONSTANT_ALPHA { get; }

            public abstract double ONE_MINUS_CONSTANT_COLOR { get; }

            public abstract double ONE_MINUS_DST_ALPHA { get; }

            public abstract double ONE_MINUS_DST_COLOR { get; }

            public abstract double ONE_MINUS_SRC_ALPHA { get; }

            public abstract double ONE_MINUS_SRC_COLOR { get; }

            public abstract double OUT_OF_MEMORY { get; }

            public abstract double PACK_ALIGNMENT { get; }

            public abstract double POINTS { get; }

            public abstract double POLYGON_OFFSET_FACTOR { get; }

            public abstract double POLYGON_OFFSET_FILL { get; }

            public abstract double POLYGON_OFFSET_UNITS { get; }

            public abstract double RED_BITS { get; }

            public abstract double RENDERBUFFER { get; }

            public abstract double RENDERBUFFER_ALPHA_SIZE { get; }

            public abstract double RENDERBUFFER_BINDING { get; }

            public abstract double RENDERBUFFER_BLUE_SIZE { get; }

            public abstract double RENDERBUFFER_DEPTH_SIZE { get; }

            public abstract double RENDERBUFFER_GREEN_SIZE { get; }

            public abstract double RENDERBUFFER_HEIGHT { get; }

            public abstract double RENDERBUFFER_INTERNAL_FORMAT { get; }

            public abstract double RENDERBUFFER_RED_SIZE { get; }

            public abstract double RENDERBUFFER_STENCIL_SIZE { get; }

            public abstract double RENDERBUFFER_WIDTH { get; }

            public abstract double RENDERER { get; }

            public abstract double REPEAT { get; }

            public abstract double REPLACE { get; }

            public abstract double RGB { get; }

            public abstract double RGB565 { get; }

            public abstract double RGB5_A1 { get; }

            public abstract double RGBA { get; }

            public abstract double RGBA4 { get; }

            public abstract double SAMPLER_2D { get; }

            public abstract double SAMPLER_CUBE { get; }

            public abstract double SAMPLES { get; }

            public abstract double SAMPLE_ALPHA_TO_COVERAGE { get; }

            public abstract double SAMPLE_BUFFERS { get; }

            public abstract double SAMPLE_COVERAGE { get; }

            public abstract double SAMPLE_COVERAGE_INVERT { get; }

            public abstract double SAMPLE_COVERAGE_VALUE { get; }

            public abstract double SCISSOR_BOX { get; }

            public abstract double SCISSOR_TEST { get; }

            public abstract double SHADER_TYPE { get; }

            public abstract double SHADING_LANGUAGE_VERSION { get; }

            public abstract double SHORT { get; }

            public abstract double SRC_ALPHA { get; }

            public abstract double SRC_ALPHA_SATURATE { get; }

            public abstract double SRC_COLOR { get; }

            public abstract double STATIC_DRAW { get; }

            public abstract double STENCIL_ATTACHMENT { get; }

            public abstract double STENCIL_BACK_FAIL { get; }

            public abstract double STENCIL_BACK_FUNC { get; }

            public abstract double STENCIL_BACK_PASS_DEPTH_FAIL { get; }

            public abstract double STENCIL_BACK_PASS_DEPTH_PASS { get; }

            public abstract double STENCIL_BACK_REF { get; }

            public abstract double STENCIL_BACK_VALUE_MASK { get; }

            public abstract double STENCIL_BACK_WRITEMASK { get; }

            public abstract double STENCIL_BITS { get; }

            public abstract double STENCIL_BUFFER_BIT { get; }

            public abstract double STENCIL_CLEAR_VALUE { get; }

            public abstract double STENCIL_FAIL { get; }

            public abstract double STENCIL_FUNC { get; }

            public abstract double STENCIL_INDEX { get; }

            public abstract double STENCIL_INDEX8 { get; }

            public abstract double STENCIL_PASS_DEPTH_FAIL { get; }

            public abstract double STENCIL_PASS_DEPTH_PASS { get; }

            public abstract double STENCIL_REF { get; }

            public abstract double STENCIL_TEST { get; }

            public abstract double STENCIL_VALUE_MASK { get; }

            public abstract double STENCIL_WRITEMASK { get; }

            public abstract double STREAM_DRAW { get; }

            public abstract double SUBPIXEL_BITS { get; }

            public abstract double TEXTURE { get; }

            public abstract double TEXTURE0 { get; }

            public abstract double TEXTURE1 { get; }

            public abstract double TEXTURE10 { get; }

            public abstract double TEXTURE11 { get; }

            public abstract double TEXTURE12 { get; }

            public abstract double TEXTURE13 { get; }

            public abstract double TEXTURE14 { get; }

            public abstract double TEXTURE15 { get; }

            public abstract double TEXTURE16 { get; }

            public abstract double TEXTURE17 { get; }

            public abstract double TEXTURE18 { get; }

            public abstract double TEXTURE19 { get; }

            public abstract double TEXTURE2 { get; }

            public abstract double TEXTURE20 { get; }

            public abstract double TEXTURE21 { get; }

            public abstract double TEXTURE22 { get; }

            public abstract double TEXTURE23 { get; }

            public abstract double TEXTURE24 { get; }

            public abstract double TEXTURE25 { get; }

            public abstract double TEXTURE26 { get; }

            public abstract double TEXTURE27 { get; }

            public abstract double TEXTURE28 { get; }

            public abstract double TEXTURE29 { get; }

            public abstract double TEXTURE3 { get; }

            public abstract double TEXTURE30 { get; }

            public abstract double TEXTURE31 { get; }

            public abstract double TEXTURE4 { get; }

            public abstract double TEXTURE5 { get; }

            public abstract double TEXTURE6 { get; }

            public abstract double TEXTURE7 { get; }

            public abstract double TEXTURE8 { get; }

            public abstract double TEXTURE9 { get; }

            public abstract double TEXTURE_2D { get; }

            public abstract double TEXTURE_BINDING_2D { get; }

            public abstract double TEXTURE_BINDING_CUBE_MAP { get; }

            public abstract double TEXTURE_CUBE_MAP { get; }

            public abstract double TEXTURE_CUBE_MAP_NEGATIVE_X { get; }

            public abstract double TEXTURE_CUBE_MAP_NEGATIVE_Y { get; }

            public abstract double TEXTURE_CUBE_MAP_NEGATIVE_Z { get; }

            public abstract double TEXTURE_CUBE_MAP_POSITIVE_X { get; }

            public abstract double TEXTURE_CUBE_MAP_POSITIVE_Y { get; }

            public abstract double TEXTURE_CUBE_MAP_POSITIVE_Z { get; }

            public abstract double TEXTURE_MAG_FILTER { get; }

            public abstract double TEXTURE_MIN_FILTER { get; }

            public abstract double TEXTURE_WRAP_S { get; }

            public abstract double TEXTURE_WRAP_T { get; }

            public abstract double TRIANGLES { get; }

            public abstract double TRIANGLE_FAN { get; }

            public abstract double TRIANGLE_STRIP { get; }

            public abstract double UNPACK_ALIGNMENT { get; }

            public abstract double UNPACK_COLORSPACE_CONVERSION_WEBGL { get; }

            public abstract double UNPACK_FLIP_Y_WEBGL { get; }

            public abstract double UNPACK_PREMULTIPLY_ALPHA_WEBGL { get; }

            public abstract double UNSIGNED_BYTE { get; }

            public abstract double UNSIGNED_INT { get; }

            public abstract double UNSIGNED_SHORT { get; }

            public abstract double UNSIGNED_SHORT_4_4_4_4 { get; }

            public abstract double UNSIGNED_SHORT_5_5_5_1 { get; }

            public abstract double UNSIGNED_SHORT_5_6_5 { get; }

            public abstract double VALIDATE_STATUS { get; }

            public abstract double VENDOR { get; }

            public abstract double VERSION { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_BUFFER_BINDING { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_ENABLED { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_NORMALIZED { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_POINTER { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_SIZE { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_STRIDE { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_TYPE { get; }

            public abstract double VERTEX_SHADER { get; }

            public abstract double VIEWPORT { get; }

            public abstract double ZERO { get; }

            public abstract double READ_BUFFER { get; }

            public abstract double UNPACK_ROW_LENGTH { get; }

            public abstract double UNPACK_SKIP_ROWS { get; }

            public abstract double UNPACK_SKIP_PIXELS { get; }

            public abstract double PACK_ROW_LENGTH { get; }

            public abstract double PACK_SKIP_ROWS { get; }

            public abstract double PACK_SKIP_PIXELS { get; }

            public abstract double COLOR { get; }

            public abstract double DEPTH { get; }

            public abstract double STENCIL { get; }

            public abstract double RED { get; }

            public abstract double RGB8 { get; }

            public abstract double RGBA8 { get; }

            public abstract double RGB10_A2 { get; }

            public abstract double TEXTURE_BINDING_3D { get; }

            public abstract double UNPACK_SKIP_IMAGES { get; }

            public abstract double UNPACK_IMAGE_HEIGHT { get; }

            public abstract double TEXTURE_3D { get; }

            public abstract double TEXTURE_WRAP_R { get; }

            public abstract double MAX_3D_TEXTURE_SIZE { get; }

            public abstract double UNSIGNED_INT_2_10_10_10_REV { get; }

            public abstract double MAX_ELEMENTS_VERTICES { get; }

            public abstract double MAX_ELEMENTS_INDICES { get; }

            public abstract double TEXTURE_MIN_LOD { get; }

            public abstract double TEXTURE_MAX_LOD { get; }

            public abstract double TEXTURE_BASE_LEVEL { get; }

            public abstract double TEXTURE_MAX_LEVEL { get; }

            public abstract double MIN { get; }

            public abstract double MAX { get; }

            public abstract double DEPTH_COMPONENT24 { get; }

            public abstract double MAX_TEXTURE_LOD_BIAS { get; }

            public abstract double TEXTURE_COMPARE_MODE { get; }

            public abstract double TEXTURE_COMPARE_FUNC { get; }

            public abstract double CURRENT_QUERY { get; }

            public abstract double QUERY_RESULT { get; }

            public abstract double QUERY_RESULT_AVAILABLE { get; }

            public abstract double STREAM_READ { get; }

            public abstract double STREAM_COPY { get; }

            public abstract double STATIC_READ { get; }

            public abstract double STATIC_COPY { get; }

            public abstract double DYNAMIC_READ { get; }

            public abstract double DYNAMIC_COPY { get; }

            public abstract double MAX_DRAW_BUFFERS { get; }

            public abstract double DRAW_BUFFER0 { get; }

            public abstract double DRAW_BUFFER1 { get; }

            public abstract double DRAW_BUFFER2 { get; }

            public abstract double DRAW_BUFFER3 { get; }

            public abstract double DRAW_BUFFER4 { get; }

            public abstract double DRAW_BUFFER5 { get; }

            public abstract double DRAW_BUFFER6 { get; }

            public abstract double DRAW_BUFFER7 { get; }

            public abstract double DRAW_BUFFER8 { get; }

            public abstract double DRAW_BUFFER9 { get; }

            public abstract double DRAW_BUFFER10 { get; }

            public abstract double DRAW_BUFFER11 { get; }

            public abstract double DRAW_BUFFER12 { get; }

            public abstract double DRAW_BUFFER13 { get; }

            public abstract double DRAW_BUFFER14 { get; }

            public abstract double DRAW_BUFFER15 { get; }

            public abstract double MAX_FRAGMENT_UNIFORM_COMPONENTS { get; }

            public abstract double MAX_VERTEX_UNIFORM_COMPONENTS { get; }

            public abstract double SAMPLER_3D { get; }

            public abstract double SAMPLER_2D_SHADOW { get; }

            public abstract double FRAGMENT_SHADER_DERIVATIVE_HINT { get; }

            public abstract double PIXEL_PACK_BUFFER { get; }

            public abstract double PIXEL_UNPACK_BUFFER { get; }

            public abstract double PIXEL_PACK_BUFFER_BINDING { get; }

            public abstract double PIXEL_UNPACK_BUFFER_BINDING { get; }

            public abstract double FLOAT_MAT2x3 { get; }

            public abstract double FLOAT_MAT2x4 { get; }

            public abstract double FLOAT_MAT3x2 { get; }

            public abstract double FLOAT_MAT3x4 { get; }

            public abstract double FLOAT_MAT4x2 { get; }

            public abstract double FLOAT_MAT4x3 { get; }

            public abstract double SRGB { get; }

            public abstract double SRGB8 { get; }

            public abstract double SRGB8_ALPHA8 { get; }

            public abstract double COMPARE_REF_TO_TEXTURE { get; }

            public abstract double RGBA32F { get; }

            public abstract double RGB32F { get; }

            public abstract double RGBA16F { get; }

            public abstract double RGB16F { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_INTEGER { get; }

            public abstract double MAX_ARRAY_TEXTURE_LAYERS { get; }

            public abstract double MIN_PROGRAM_TEXEL_OFFSET { get; }

            public abstract double MAX_PROGRAM_TEXEL_OFFSET { get; }

            public abstract double MAX_VARYING_COMPONENTS { get; }

            public abstract double TEXTURE_2D_ARRAY { get; }

            public abstract double TEXTURE_BINDING_2D_ARRAY { get; }

            public abstract double R11F_G11F_B10F { get; }

            public abstract double UNSIGNED_INT_10F_11F_11F_REV { get; }

            public abstract double RGB9_E5 { get; }

            public abstract double UNSIGNED_INT_5_9_9_9_REV { get; }

            public abstract double TRANSFORM_FEEDBACK_BUFFER_MODE { get; }

            public abstract double MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS { get; }

            public abstract double TRANSFORM_FEEDBACK_VARYINGS { get; }

            public abstract double TRANSFORM_FEEDBACK_BUFFER_START { get; }

            public abstract double TRANSFORM_FEEDBACK_BUFFER_SIZE { get; }

            public abstract double TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN { get; }

            public abstract double RASTERIZER_DISCARD { get; }

            public abstract double MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS { get; }

            public abstract double MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS { get; }

            public abstract double INTERLEAVED_ATTRIBS { get; }

            public abstract double SEPARATE_ATTRIBS { get; }

            public abstract double TRANSFORM_FEEDBACK_BUFFER { get; }

            public abstract double TRANSFORM_FEEDBACK_BUFFER_BINDING { get; }

            public abstract double RGBA32UI { get; }

            public abstract double RGB32UI { get; }

            public abstract double RGBA16UI { get; }

            public abstract double RGB16UI { get; }

            public abstract double RGBA8UI { get; }

            public abstract double RGB8UI { get; }

            public abstract double RGBA32I { get; }

            public abstract double RGB32I { get; }

            public abstract double RGBA16I { get; }

            public abstract double RGB16I { get; }

            public abstract double RGBA8I { get; }

            public abstract double RGB8I { get; }

            public abstract double RED_INTEGER { get; }

            public abstract double RGB_INTEGER { get; }

            public abstract double RGBA_INTEGER { get; }

            public abstract double SAMPLER_2D_ARRAY { get; }

            public abstract double SAMPLER_2D_ARRAY_SHADOW { get; }

            public abstract double SAMPLER_CUBE_SHADOW { get; }

            public abstract double UNSIGNED_INT_VEC2 { get; }

            public abstract double UNSIGNED_INT_VEC3 { get; }

            public abstract double UNSIGNED_INT_VEC4 { get; }

            public abstract double INT_SAMPLER_2D { get; }

            public abstract double INT_SAMPLER_3D { get; }

            public abstract double INT_SAMPLER_CUBE { get; }

            public abstract double INT_SAMPLER_2D_ARRAY { get; }

            public abstract double UNSIGNED_INT_SAMPLER_2D { get; }

            public abstract double UNSIGNED_INT_SAMPLER_3D { get; }

            public abstract double UNSIGNED_INT_SAMPLER_CUBE { get; }

            public abstract double UNSIGNED_INT_SAMPLER_2D_ARRAY { get; }

            public abstract double DEPTH_COMPONENT32F { get; }

            public abstract double DEPTH32F_STENCIL8 { get; }

            public abstract double FLOAT_32_UNSIGNED_INT_24_8_REV { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_RED_SIZE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_GREEN_SIZE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_BLUE_SIZE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE { get; }

            public abstract double FRAMEBUFFER_DEFAULT { get; }

            public abstract double UNSIGNED_INT_24_8 { get; }

            public abstract double DEPTH24_STENCIL8 { get; }

            public abstract double UNSIGNED_NORMALIZED { get; }

            public abstract double DRAW_FRAMEBUFFER_BINDING { get; }

            public abstract double READ_FRAMEBUFFER { get; }

            public abstract double DRAW_FRAMEBUFFER { get; }

            public abstract double READ_FRAMEBUFFER_BINDING { get; }

            public abstract double RENDERBUFFER_SAMPLES { get; }

            public abstract double FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER { get; }

            public abstract double MAX_COLOR_ATTACHMENTS { get; }

            public abstract double COLOR_ATTACHMENT1 { get; }

            public abstract double COLOR_ATTACHMENT2 { get; }

            public abstract double COLOR_ATTACHMENT3 { get; }

            public abstract double COLOR_ATTACHMENT4 { get; }

            public abstract double COLOR_ATTACHMENT5 { get; }

            public abstract double COLOR_ATTACHMENT6 { get; }

            public abstract double COLOR_ATTACHMENT7 { get; }

            public abstract double COLOR_ATTACHMENT8 { get; }

            public abstract double COLOR_ATTACHMENT9 { get; }

            public abstract double COLOR_ATTACHMENT10 { get; }

            public abstract double COLOR_ATTACHMENT11 { get; }

            public abstract double COLOR_ATTACHMENT12 { get; }

            public abstract double COLOR_ATTACHMENT13 { get; }

            public abstract double COLOR_ATTACHMENT14 { get; }

            public abstract double COLOR_ATTACHMENT15 { get; }

            public abstract double FRAMEBUFFER_INCOMPLETE_MULTISAMPLE { get; }

            public abstract double MAX_SAMPLES { get; }

            public abstract double HALF_FLOAT { get; }

            public abstract double RG { get; }

            public abstract double RG_INTEGER { get; }

            public abstract double R8 { get; }

            public abstract double RG8 { get; }

            public abstract double R16F { get; }

            public abstract double R32F { get; }

            public abstract double RG16F { get; }

            public abstract double RG32F { get; }

            public abstract double R8I { get; }

            public abstract double R8UI { get; }

            public abstract double R16I { get; }

            public abstract double R16UI { get; }

            public abstract double R32I { get; }

            public abstract double R32UI { get; }

            public abstract double RG8I { get; }

            public abstract double RG8UI { get; }

            public abstract double RG16I { get; }

            public abstract double RG16UI { get; }

            public abstract double RG32I { get; }

            public abstract double RG32UI { get; }

            public abstract double VERTEX_ARRAY_BINDING { get; }

            public abstract double R8_SNORM { get; }

            public abstract double RG8_SNORM { get; }

            public abstract double RGB8_SNORM { get; }

            public abstract double RGBA8_SNORM { get; }

            public abstract double SIGNED_NORMALIZED { get; }

            public abstract double COPY_READ_BUFFER { get; }

            public abstract double COPY_WRITE_BUFFER { get; }

            public abstract double COPY_READ_BUFFER_BINDING { get; }

            public abstract double COPY_WRITE_BUFFER_BINDING { get; }

            public abstract double UNIFORM_BUFFER { get; }

            public abstract double UNIFORM_BUFFER_BINDING { get; }

            public abstract double UNIFORM_BUFFER_START { get; }

            public abstract double UNIFORM_BUFFER_SIZE { get; }

            public abstract double MAX_VERTEX_UNIFORM_BLOCKS { get; }

            public abstract double MAX_FRAGMENT_UNIFORM_BLOCKS { get; }

            public abstract double MAX_COMBINED_UNIFORM_BLOCKS { get; }

            public abstract double MAX_UNIFORM_BUFFER_BINDINGS { get; }

            public abstract double MAX_UNIFORM_BLOCK_SIZE { get; }

            public abstract double MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS { get; }

            public abstract double MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS { get; }

            public abstract double UNIFORM_BUFFER_OFFSET_ALIGNMENT { get; }

            public abstract double ACTIVE_UNIFORM_BLOCKS { get; }

            public abstract double UNIFORM_TYPE { get; }

            public abstract double UNIFORM_SIZE { get; }

            public abstract double UNIFORM_BLOCK_INDEX { get; }

            public abstract double UNIFORM_OFFSET { get; }

            public abstract double UNIFORM_ARRAY_STRIDE { get; }

            public abstract double UNIFORM_MATRIX_STRIDE { get; }

            public abstract double UNIFORM_IS_ROW_MAJOR { get; }

            public abstract double UNIFORM_BLOCK_BINDING { get; }

            public abstract double UNIFORM_BLOCK_DATA_SIZE { get; }

            public abstract double UNIFORM_BLOCK_ACTIVE_UNIFORMS { get; }

            public abstract double UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES { get; }

            public abstract double UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER { get; }

            public abstract double UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER { get; }

            public abstract double INVALID_INDEX { get; }

            public abstract double MAX_VERTEX_OUTPUT_COMPONENTS { get; }

            public abstract double MAX_FRAGMENT_INPUT_COMPONENTS { get; }

            public abstract double MAX_SERVER_WAIT_TIMEOUT { get; }

            public abstract double OBJECT_TYPE { get; }

            public abstract double SYNC_CONDITION { get; }

            public abstract double SYNC_STATUS { get; }

            public abstract double SYNC_FLAGS { get; }

            public abstract double SYNC_FENCE { get; }

            public abstract double SYNC_GPU_COMMANDS_COMPLETE { get; }

            public abstract double UNSIGNALED { get; }

            public abstract double SIGNALED { get; }

            public abstract double ALREADY_SIGNALED { get; }

            public abstract double TIMEOUT_EXPIRED { get; }

            public abstract double CONDITION_SATISFIED { get; }

            public abstract double WAIT_FAILED { get; }

            public abstract double SYNC_FLUSH_COMMANDS_BIT { get; }

            public abstract double VERTEX_ATTRIB_ARRAY_DIVISOR { get; }

            public abstract double ANY_SAMPLES_PASSED { get; }

            public abstract double ANY_SAMPLES_PASSED_CONSERVATIVE { get; }

            public abstract double SAMPLER_BINDING { get; }

            public abstract double RGB10_A2UI { get; }

            public abstract double INT_2_10_10_10_REV { get; }

            public abstract double TRANSFORM_FEEDBACK { get; }

            public abstract double TRANSFORM_FEEDBACK_PAUSED { get; }

            public abstract double TRANSFORM_FEEDBACK_ACTIVE { get; }

            public abstract double TRANSFORM_FEEDBACK_BINDING { get; }

            public abstract double TEXTURE_IMMUTABLE_FORMAT { get; }

            public abstract double MAX_ELEMENT_INDEX { get; }

            public abstract double TEXTURE_IMMUTABLE_LEVELS { get; }

            public abstract double TIMEOUT_IGNORED { get; }

            public abstract double MAX_CLIENT_WAIT_TIMEOUT_WEBGL { get; }

            protected WebGL2RenderingContextTypeConfig() : base()
            {
            }
        }

        [Virtual]
        public abstract class WebGLQueryTypeConfig : IObject
        {

            public virtual webgl2.WebGLQuery prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract webgl2.WebGLQuery New();

            protected WebGLQueryTypeConfig() : base()
            {
            }
        }

        [Virtual]
        public abstract class WebGLSamplerTypeConfig : IObject
        {

            public virtual webgl2.WebGLSampler prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract webgl2.WebGLSampler New();

            protected WebGLSamplerTypeConfig() : base()
            {
            }
        }

        [Virtual]
        public abstract class WebGLSyncTypeConfig : IObject
        {

            public virtual webgl2.WebGLSync prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract webgl2.WebGLSync New();

            protected WebGLSyncTypeConfig() : base()
            {
            }
        }

        [Virtual]
        public abstract class WebGLTransformFeedbackTypeConfig : IObject
        {

            public virtual webgl2.WebGLTransformFeedback prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract webgl2.WebGLTransformFeedback New();

            protected WebGLTransformFeedbackTypeConfig() : base()
            {
            }
        }

        [Virtual]
        public abstract class WebGLVertexArrayObjectTypeConfig : IObject
        {

            public virtual webgl2.WebGLVertexArrayObject prototype
            {
                get;
                set;
            }

            [Template("new {this}()")]
            public abstract webgl2.WebGLVertexArrayObject New();

            protected WebGLVertexArrayObjectTypeConfig() : base()
            {
            }
        }

        public static class Literals
        {
            [Template("<self>\"webgl2\"")]
            public static readonly webgl2.Literals.Types.webgl2 webgl2;
            [Template("<self>\"experimental-webgl2\"")]
            public static readonly webgl2.Literals.Types.experimental_webgl2 experimental_webgl2;

            public static class Types
            {
                [Name("System.String")]
                public class webgl2 : LiteralType<string>
                {
                    private extern webgl2();
                }

                [Name("System.String")]
                public class experimental_webgl2 : LiteralType<string>
                {
                    private extern experimental_webgl2();
                }
            }

            public static class Options
            {
                [Name("System.String")]
                public class contextId : LiteralType<string>
                {
                    [Template("<self>\"webgl2\"")]
                    public static readonly webgl2.Literals.Types.webgl2 webgl2;
                    [Template("<self>\"experimental-webgl2\"")]
                    public static readonly webgl2.Literals.Types.experimental_webgl2 experimental_webgl2;

                    private extern contextId();

                    public static extern implicit operator webgl2.Literals.Options.contextId(
                      webgl2.Literals.Types.webgl2 value);

                    public static extern implicit operator webgl2.Literals.Options.contextId(
                      webgl2.Literals.Types.experimental_webgl2 value);
                }
            }
        }
    }
}
