using System.Runtime.InteropServices;

namespace Reloaded.Memory.Example.Structs
{
    /// <summary>
    /// A structure representing the physics data layout for Sonic Adventure, Sonic Adventure DX, Sonic Adventure 2,
    /// Sonic Adventure 2 Battle, Sonic Heroes. Phys.bin contains an array of this structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AdventurePhysics
    {
        /// <summary>
        /// Amount of frames in which the character will fall at a decreased rate when rolling off a ledge.
        /// </summary>
        public int HangTime;

        /// <summary>
        /// Higher values indicate easier acceleration on rough surfaces, rarely used in Heroes.
        /// </summary>
        public float FloorGrip;

        /// <summary>
        /// The horizontal speed cap, maximum speed in X/Z axis.
        /// </summary>
        public float HorizontalSpeedCap;

        /// <summary>
        /// The vertical speed cap, maximum speed in Y axis.
        /// </summary>
        public float VerticalSpeedCap;

        /// <summary>
        /// Affects character acceleration.
        /// </summary>
        public float UnknownAccelRelated;

        /// <summary>
        /// Unknown. This value is same as in SADX.
        /// </summary>
        public float Unknown1;

        /// <summary>
        /// The initial vertical speed set by the game when the player presses the "Jump" button.
        /// </summary>
        public float InitialJumpSpeed;

        /// <summary>
        /// Seems related to springs (at least in SADX), unknown.
        /// </summary>
        public float SpringControl;

        /// <summary>
        /// Unknown value.
        /// </summary>
        public float Unknown2;

        /// <summary>
        /// If the character is below this speed, the roll will be cancelled.
        /// </summary>
        public float RollingMinimumSpeed;

        /// <summary>
        /// Unknown value. SADX/SA2: Rolling End Speed
        /// </summary>
        public float RollingEndSpeed;

        /// <summary>
        /// Speed after starting to roll as Sonic or punching as Knuckles. SADX: Running_I Speed
        /// </summary>
        public float Action1Speed;

        /// <summary>
        /// The minimum speed of knockback/push in another direction when making contact with a wall.
        /// </summary>
        public float MinWallHitKnockbackSpeed;

        /// <summary>
        /// Speed after kicking as Sonic or punching as Knuckles.
        /// </summary>
        public float Action2Speed;

        /// <summary>
        /// While jump is held, this is added to speed. Allows for higher jumps when holding jump.
        /// </summary>
        public float JumpHoldAddSpeed;

        /// <summary>
        /// The character's ground horizontal acceleration. Speed is set to this value when starting from a neutral position and is applied constantly until the character reaches a set speed.
        /// </summary>
        public float GroundStartingAcceleration;

        /// <summary>
        /// How fast the character gains speed in the air.
        /// </summary>
        public float AirAcceleration;

        /// <summary>
        /// How fast the character decelerates naturally on the ground when no buttons are held.
        /// </summary>
        public float GroundDeceleration;

        /// <summary>
        /// How fast the character can halt on the ground after holding the direction opposite to direction of travel after running.
        /// </summary>
        public float BrakeSpeed;

        /// <summary>
        /// How fast the character can halt in the air when holding the direction opposite to direction of travel.
        /// </summary>
        public float AirBrakeSpeed;

        /// <summary>
        /// How fast the character decelerates naturally in the air when no buttons are held.
        /// </summary>
        public float AirDeceleration;

        /// <summary>
        /// How fast the character decelerates naturally in the air when no buttons are held.
        /// </summary>
        public float RollingDeceleration;

        /// <summary>
        /// This speed is added every frame in the direction that the character is travelling in the Y Axis. e.g. If you are going up, a positive value will push you up but will push you down when falling.
        /// </summary>
        public float GravityOffsetSpeed;

        /// <summary>
        /// (Seems to be tied with collision?) The speed applied to Sonic when he tried to alter the course of his original trajectory by pushing left or right in mid air.
        /// </summary>
        public float MidAirSwerveSpeed;

        /// <summary>
        /// The minimum speed until the character will stop moving completely.
        /// </summary>
        public float MinSpeedBeforeStopping;

        /// <summary>
        /// Constant force applied to the left of the character, used to make characters appear to run sideways. NOT IN OTHER SANIC GAMES.
        /// </summary>
        public float ConstantSpeedOffset;

        /// <summary>
        /// Unknown value, affects off road acceleration. The closer to -0 on the negative, the slower the offroad acceleration.
        /// </summary>
        public float UnknownOffRoad;

        public float Unknown3;
        public float Unknown4;

        /// <summary>
        /// Represents the radius of the collision cylinder which represents Sonic.
        /// </summary>
        public float CollisionSize;

        /// <summary>
        /// Gravity Constant for the character.
        /// </summary>
        public float GravitationalPull;

        /// <summary>
        /// (Only affects when playing as partner?) Y_Offset for camera.
        /// </summary>
        public float YOffsetCamera;

        /// <summary>
        /// (Only affects when playing as partner?) Physical location Y_Offset
        /// </summary>
        public float YOffset;
    }
}
