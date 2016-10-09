using System;
using Microsoft.Xna.Framework;

namespace MonogameLearning.Utilities
{
    public static class ExtensionUtilities
    {
        /*
        http://gamedev.stackexchange.com/questions/15070/orienting-a-model-to-face-a-target
        Really helped with getting this to work, after spending hours implementing and testing.
        http://answers.unity3d.com/questions/534907/difficulty-understanding-lookrotation.html
        http://gamedev.stackexchange.com/questions/15070/orienting-a-model-to-face-a-target
        http://stackoverflow.com/questions/12435671/quaternion-lookat-function
        */
        public static Quaternion LookRotation(this Quaternion q, Vector3 from, Vector3 to, Vector3 up)
        {
            Vector3 rotAxis;
            float rotAngle, dot;

            dot = Vector3.Dot(from, to);

            if(Math.Abs(dot + 1.0f) < 0.00001f)
            {
                return Quaternion.CreateFromAxisAngle(up, MathHelper.ToRadians(180));
            }
            else if(Math.Abs(dot - 1.0f) < 0.00001f)
            {
                return Quaternion.Identity;
            }

            rotAngle = (float)Math.Acos(dot);
            rotAxis = Vector3.Cross(from, to);
            rotAxis.Normalize();
            return Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
        }

        /// <summary>
        /// Needs fixing. Maybe go with y,x,z version instead of z,x,y
        /// ref links: http://answers.unity3d.com/questions/416169/finding-pitchrollyaw-from-quaternions.html
        /// http://gamedev.stackexchange.com/questions/50963/how-to-extract-euler-angles-from-transformation-matrix
        /// https://en.wikipedia.org/wiki/Rotation_formalisms_in_three_dimensions#Conversion_formulae_between_formalisms
        /// http://bediyap.com/programming/convert-quaternion-to-euler-rotations/
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Vector3 toEuler(this Quaternion q)
        {
            Vector3 euler;
            float qxs, qys, qzs, qws;
            qxs = q.X * q.X;
            qys = q.Y * q.Y;
            qzs = q.Z * q.Z;
            qws = q.W * q.W;

            /* y,x,z
                     2*(q.x*q.z + q.w*q.y),
                     q.w*q.w - q.x*q.x - q.y*q.y + q.z*q.z,
                    -2*(q.y*q.z - q.w*q.x),
                     2*(q.x*q.y + q.w*q.z),
                     q.w*q.w - q.x*q.x + q.y*q.y - q.z*q.z,

                roll  = Mathf.Atan2(2*y*w - 2*x*z, 1 - 2*y*y - 2*z*z);
                pitch = Mathf.Atan2(2*x*w - 2*y*z, 1 - 2*x*x - 2*z*z);
                yaw   =  Mathf.Asin(2*x*y + 2*z*w);

             z, x, y
                     -2*(q.x*q.y - q.w*q.z),
                      q.w*q.w - q.x*q.x + q.y*q.y - q.z*q.z,
                      2*(q.y*q.z + q.w*q.x),
                     -2*(q.x*q.z - q.w*q.y),
                      q.w*q.w - q.x*q.x - q.y*q.y + q.z*q.z,
            */

            float[] rotComponents = new float[5];
            rotComponents[0] = (q.X * q.Y - q.W * q.Z) * -2;
            rotComponents[1] = qws - qxs + qys - qzs;
            rotComponents[2] = (q.Y * q.Z + q.W * q.X) * 2;
            rotComponents[3] = (q.X * q.Z - q.W * q.Y) * -2;
            rotComponents[4] = qws - qxs - qys + qzs;

            euler.X = (float)Math.Atan2(rotComponents[0], rotComponents[1]);
            euler.Y = (float)Math.Atan2(rotComponents[3], rotComponents[4]);
            euler.Z = (float)Math.Asin(rotComponents[2]);

            euler.X = MathHelper.ToDegrees(euler.X);
            euler.Y = MathHelper.ToDegrees(euler.Y);
            euler.Z = MathHelper.ToDegrees(euler.Z);

            return euler;
        }
    }
}
