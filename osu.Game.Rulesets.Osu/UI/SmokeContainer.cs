﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Osu.Skinning;
using osu.Game.Rulesets.Osu.Skinning.Default;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Osu.UI
{
    /// <summary>
    /// Manages smoke trails generated from user input.
    /// </summary>
    public class SmokeContainer : Container, IRequireHighFrequencyMousePosition, IKeyBindingHandler<OsuAction>
    {
        private SkinnableDrawable? currentSegmentSkinnable;

        private Vector2 lastMousePosition;

        public override bool ReceivePositionalInputAt(Vector2 _) => true;

        public bool OnPressed(KeyBindingPressEvent<OsuAction> e)
        {
            if (e.Action == OsuAction.Smoke)
            {
                AddInternal(currentSegmentSkinnable = new SkinnableDrawable(new OsuSkinComponent(OsuSkinComponents.SmokeTrail), _ => new DefaultSmokeSegment()));

                // Add initial position immediately.
                addPosition();
                return true;
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<OsuAction> e)
        {
            if (e.Action == OsuAction.Smoke)
            {
                if (currentSegmentSkinnable?.Drawable is SmokeSegment segment)
                {
                    segment.FinishDrawing(Time.Current);

                    currentSegmentSkinnable.LifetimeEnd = segment.LifetimeEnd;
                    currentSegmentSkinnable = null;
                }
            }
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            lastMousePosition = e.MousePosition;
            addPosition();

            return base.OnMouseMove(e);
        }

        private void addPosition() => (currentSegmentSkinnable?.Drawable as SmokeSegment)?.AddPosition(lastMousePosition, Time.Current);
    }
}
