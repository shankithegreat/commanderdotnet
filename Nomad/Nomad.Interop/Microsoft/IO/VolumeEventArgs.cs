namespace Microsoft.IO
{
    using System;
    using System.IO;

    public class VolumeEventArgs : EventArgs
    {
        public readonly DriveChars DriveChar;
        private DriveInfo FDrive;
        private VolumeInfo FVolume;

        public VolumeEventArgs(DriveChars driveChar)
        {
            this.DriveChar = driveChar;
        }

        public DriveInfo Drive
        {
            get
            {
                if (this.FDrive == null)
                {
                    this.FDrive = new DriveInfo(this.DriveName);
                }
                return this.FDrive;
            }
        }

        public string DriveName
        {
            get
            {
                return (this.DriveChar.ToString() + @":\");
            }
        }

        public VolumeInfo Volume
        {
            get
            {
                if (this.FVolume == null)
                {
                    this.FVolume = VolumeCache.Get(this.DriveName);
                }
                return this.FVolume;
            }
        }
    }
}

