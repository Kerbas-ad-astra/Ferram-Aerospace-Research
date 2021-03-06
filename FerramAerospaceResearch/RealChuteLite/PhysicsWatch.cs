﻿using System;

/* RealChuteLite is the work of Christophe Savard (stupid_chris), and is licensed the same way than the rest of FAR is.
 * If you have any questions about this code, or want to report something, don't bug ferram about it, ask me
 * directly on GitHub, the forums, or IRC. */

namespace FerramAerospaceResearch.RealChuteLite
{
    /// <summary>
    /// A generic Stopwatch clone which runs on KSP's internal clock
    /// </summary>
    public class PhysicsWatch
    {
        #region Constants
        /// <summary>
        /// The amound of ticks in a second
        /// </summary>
        protected const long ticksPerSecond = 10000000L;

        /// <summary>
        /// The amount of milliseconds in a second
        /// </summary>
        protected const long millisecondPerSecond = 1000L;
        #endregion

        #region Fields
        /// <summary>
        /// UT of the last frame
        /// </summary>
        protected double lastCheck = 0d;

        /// <summary>
        /// Total elapsed time calculated by the watch in seconds
        /// </summary>
        protected double totalSeconds = 0d;
        #endregion

        #region Propreties
        private bool _isRunning = false;
        /// <summary>
        /// If the watch is currently counting down time
        /// </summary>
        public bool isRunning
        {
            get { return this._isRunning; }
            protected set { this._isRunning = value; }
        }

        /// <summary>
        /// The current elapsed time of the watch
        /// </summary>
        public TimeSpan elapsed
        {
            get { return new TimeSpan(this.elapsedTicks); }
        }

        /// <summary>
        /// The amount of milliseconds elapsed to the current watch
        /// </summary>
        public long elapsedMilliseconds
        {
            get
            {
                if (this._isRunning) { UpdateWatch(); }
                return (long)Math.Round(this.totalSeconds * millisecondPerSecond);
            }
        }

        /// <summary>
        /// The amount of ticks elapsed to the current watch
        /// </summary>
        public long elapsedTicks
        {
            get
            {
                if (this._isRunning) { UpdateWatch(); }
                return (long)Math.Round(this.totalSeconds * ticksPerSecond);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new PhysicsWatch
        /// </summary>
        public PhysicsWatch() { }

        /// <summary>
        /// Creates a new PhysicsWatch starting at a certain amount of time
        /// </summary>
        /// <param name="seconds">Time to start at, in seconds</param>
        public PhysicsWatch(double seconds)
        {
            this.totalSeconds = seconds;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the watch
        /// </summary>
        public void Start()
        {
            if (!this._isRunning)
            {
                this.lastCheck = Planetarium.GetUniversalTime();
                this._isRunning = true;
            }
        }

        /// <summary>
        /// Stops the watch
        /// </summary>
        public void Stop()
        {
            if (this._isRunning)
            {
                UpdateWatch();
                this._isRunning = false;
            }
        }

        /// <summary>
        /// Resets the watch to zero and starts it
        /// </summary>
        public void Restart()
        {
            this.totalSeconds = 0;
            this.lastCheck = Planetarium.GetUniversalTime();
            this._isRunning = true;
        }

        /// <summary>
        /// Stops the watch and resets it to zero
        /// </summary>
        public void Reset()
        {
            this.totalSeconds = 0;
            this.lastCheck = 0;
            this._isRunning = false;
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Updates the time on the watch
        /// </summary>
        protected virtual void UpdateWatch()
        {
            double current = Planetarium.GetUniversalTime();
            this.totalSeconds += current - this.lastCheck;
            this.lastCheck = current;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns a string representation fo this instance
        /// </summary>
        public override string ToString()
        {
            return elapsed.ToString();
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Creates a new PhysicsWatch, starts it, and returns the current instance
        /// </summary>
        public static PhysicsWatch StartNew()
        {
            PhysicsWatch watch = new PhysicsWatch();
            watch.Start();
            return watch;
        }

        /// <summary>
        /// Creates a new PhysicsWatch from a certain amount of time, starts it, and returns the current instance
        /// </summary>
        /// <param name="seconds">Time to start the watch at, in seconds</param>
        public static PhysicsWatch StartNewFromTime(double seconds)
        {
            PhysicsWatch watch = new PhysicsWatch(seconds);
            watch.Start();
            return watch;
        }
        #endregion
    }
}