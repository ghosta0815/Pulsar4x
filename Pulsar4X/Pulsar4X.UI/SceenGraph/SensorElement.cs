﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pulsar4X;
using Pulsar4X.UI;
using Pulsar4X.UI.GLUtilities;
using OpenTK;
using Pulsar4X.Entities;
using Pulsar4X.Entities.Components;

namespace Pulsar4X.UI.SceenGraph
{
    public class SensorElement : SceenElement
    {
        /// <summary>
        /// filler pointer to the sensor in question.
        /// </summary>
        public override GameEntity SceenEntity { get; set; }

        /// <summary>
        /// what is the radius of this sensor display? the label will need to know this to position itself correctly.
        /// </summary>
        private float _DisplayRadius;
        public float _displayRadius
        {
            get { return _DisplayRadius; }
            set { _DisplayRadius = value; }

        }

        /// <summary>
        /// What type of sensor is this sensor element displaying?
        /// </summary>
        private ComponentTypeTN _SensorType;
        public ComponentTypeTN _sensorType
        {
            get { return _SensorType; }
            set { _SensorType = value; }
        }

        /// <summary>
        /// What sceen does this sensor element belong to?
        /// </summary>
        private Sceen _ParentSceen { get; set; }

        //have to add new entry for GL Circle to draw sensor bubbles around taskgroups/populations/missile contacts
        //CircleElement has the circle in it already, but that might be inappropriate for what I want
        //SceenElement has the lable properly done.

        public SensorElement(GLEffect a_oDefaultEffect, Vector3 a_oPosition, float a_fRadius, System.Drawing.Color a_oColor, String LabelText, GameEntity Ent,  ComponentTypeTN SType, Sceen ParentSceenArg)
            : base()
        {
            _ParentSceen = ParentSceenArg;

            _DisplayRadius = a_fRadius;

            m_oPrimaryPrimitive = new GLCircle(a_oDefaultEffect,
                        a_oPosition,
                        a_fRadius,
                        a_oColor,
                        UIConstants.Textures.DEFAULT_TEXTURE);

            m_lPrimitives.Add(m_oPrimaryPrimitive);

            int LabelMid = LabelText.Length / 4;
            float xAdjust = -LabelMid * (UIConstants.DEFAULT_TEXT_SIZE.X / _ParentSceen.ZoomSclaer);
            float yAdjust = 10.0f / ParentSceenArg.ZoomSclaer;
            Vector3 LPos = new Vector3(xAdjust, (a_fRadius + yAdjust), 0.0f);
            LPos = LPos + a_oPosition;
            Lable = new GLUtilities.GLFont(a_oDefaultEffect, LPos, UIConstants.DEFAULT_TEXT_SIZE, a_oColor, UIConstants.Textures.DEFAULT_GLFONT2, LabelText);
            Lable.Size = UIConstants.DEFAULT_TEXT_SIZE / ParentSceenArg.ZoomSclaer;
            SetActualPosition(a_oPosition);

            SceenEntity = Ent;
            _SensorType = SType;
        }

        public override void Render()
        {
            foreach (GLPrimitive oPrimitive in m_lPrimitives)
            {
                oPrimitive.Render();
            }

            if (RenderChildren == true)
            {
                foreach (SceenElement oElement in m_lChildren)
                {
                    oElement.Render();
                }
            }

            Lable.Render();
        }

        public override void Refresh(float a_fZoomScaler)
        {
            Lable.Size = UIConstants.DEFAULT_TEXT_SIZE / a_fZoomScaler;

            // loop through any children:
            foreach (SceenElement oElement in m_lChildren)
            {
                oElement.Refresh(a_fZoomScaler);
            }
        }

        public override Guid GetSelected(Vector3 a_v3AtPos)
        {
            Guid oElementID = Guid.Empty;

            // else go though this elements children.
            foreach (SceenElement oElement in m_lChildren)
            {
                oElementID = oElement.GetSelected(a_v3AtPos);

                if (oElementID != Guid.Empty)
                {
                    // we have found something, retur its ID:
                    return oElementID;
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// SetActualPosition casts the primary primitive as a circle, tests to see if it exists, and moves it.
        /// </summary>
        /// <param name="a_v3Pos">Position to move this circle element to.</param>
        public void SetActualPosition(Vector3 a_v3Pos)
        {
            GLCircle temp = this.PrimaryPrimitive as GLCircle;

            if (temp != null)
            {
                temp.Position = a_v3Pos;
                int LabelMid = Lable.Text.Length/4;
                float xAdjust = -LabelMid * (UIConstants.DEFAULT_TEXT_SIZE.X / _ParentSceen.ZoomSclaer);
                float yAdjust = 10.0f / _ParentSceen.ZoomSclaer;
                Vector3 LPos = new Vector3(xAdjust, (_DisplayRadius + yAdjust), 0.0f);
                Lable.Position = temp.Position + LPos;

                /// <summary>
                /// Have to call this to get openGL to update temps position.
                /// </summary>
                temp.RecalculateModelMatrix();
            }
        }
    }
}
