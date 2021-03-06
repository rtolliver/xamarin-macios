﻿using System;
using System.Reflection;

#if XAMCORE_2_0
using Foundation;
#else
using MonoTouch.Foundation;
#endif

using NUnit.Framework;

namespace Introspection {

	[TestFixture]
	// we want the tests to be available because we use the linker
	[Preserve (AllMembers = true)]
	public class iOSApiWeakPropertyTest : ApiWeakPropertyTest {

		public iOSApiWeakPropertyTest ()
		{
			ContinueOnFailure = true;
		}

		protected override bool Skip (PropertyInfo property)
		{
			switch (property.DeclaringType.Name) {
			// WeakVideoGravity is an NSString that we could/should provide a better binding (e.g. enum)
			case "AVPlayerViewController":
				return property.Name == "WeakVideoGravity";
			// CATextLayer.WeakFont is done correctly by hand
			case "CATextLayer":
				return property.Name == "WeakFont";
			// NSAttributedStringDocumentAttributes is a DictionaryContainer that expose some Weak* NSDictionary
			case "NSAttributedStringDocumentAttributes":
				return property.Name == "WeakDocumentType" || property.Name == "WeakDefaultAttributes";
			// UIFontAttributes is a DictionaryContainer that expose a Weak* NSDictionary
			case "UIFontAttributes":
				return property.Name == "WeakFeatureSettings";
			// UIStringAttributes is a DictionaryContainer that expose a Weak* NSString
			case "UIStringAttributes":
				return property.Name == "WeakTextEffect";
#if !XAMCORE_3_0
			// #37451 - setter does not exists but we have to keep it for binary compatibility
			// OTOH we can't give it a selector (private API) even if we suspect Apple is mostly running `strings` on executable
			case "IUIViewControllerPreviewing":
				return property.Name == "WeakDelegate";
			case "UIViewControllerPreviewingWrapper":
				return property.Name == "WeakDelegate";
#endif
			}
			return base.Skip (property);
		}
	}
}