
Implementation Notes for the Spotlight Feed Browser
---------------------------------------------------

Comments are provided throughout, and on all interfaces and most public methods of classes.

Unit tests are incomplete; there are a number of more comprehensive tests that could be written, to address such things as:

    * large number of items in feed
    * validating that deserialized items have the correct field values
    * SpotlightFeedCache isn't unit tested at all (I ran out of time)
    * EnumToIconConverter
    * passing invalid values to CreateAsync() methods
    * large string values for fields
    * simulating network latency, various web exceptions, etc.
    * fake cached data - file corruption, insufficient file permissions, lock file during read/write, etc.

Implementation is fairly basic. If I had more time I would have considered the following enhancements:

	* JSON data could be too big to be held in memory - there isn't too much we can do about this, since 
	  breaking up the JSON data requires knowing how it's structured
	* the web client could potentially be separated into its own component, instead of being embedded into the SpotlightFeedReader
	* the factory pattern (http://blog.stephencleary.com/2013/01/async-oop-2-constructors.html) used to asynchronously 
	  create an initialize objects (in order to avoid async in constructors) seems like it might have a timing issue when the
	  asynchronously created object first initializes. I didn't have time to investigate this further.
	* the code behind in SpotlightView.xaml.cs could potentially be extracted to an extension class
	* it would probably be cleaner to have the SpotlightFeedReader return a list of SpotlightItemViewModels instead of leaking
	  knowledge about the model (SpotlightItemRoot) to the main view model
	* the retry logic is a bit clunky. I could have had it return the items (asynchronously) instead of requiring another call
	  to GetFeed() but I didn't like the idea of having two ways of getting the data, effectively. I would need to give this
	  a bit more thought to come up with something cleaner
	* if there are a lot of items in the feed, the SpotlightFeedReader could return a batch at a time, enabling the listview to
	  begin populating itself before all items are added, and additionally data virtualization could be implemented to ensure
	  we don't keep unnecessary objects in memory
	* when operating in 'offline mode' the images aren't available (but the rest of the metadata has been cached) - I could down-
	  load the images before deserializing and/or add a 'missing image' placeholder in the UI