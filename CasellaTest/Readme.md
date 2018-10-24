# TextInputStatistics
WebApi accepting an HTTP POST with raw body encoded as UTF-8 text.

# How to run locally.

- Project is setup by default to run in IIS Express, or can be installed as an application in standalone IIS
- Accepts HTTP POST, with raw text in Body for data input.

# Potential Errors
- most input errors should be handled.
- 400 error is returned if an empty input string is passed in the POST body
- any unexpected and unhandled exception will give a 400 error and the exception detail will be passed to the 400 error's ReasonPhrase.  

# Potential Limits
- maximum request size set at 50MB.  If larger files are needed then the WebApi will need to be configured for streaming.  As these are text
documents, it should be reasonable to assume that individual file sizes may stay at the lower end, but if traffic increases implementing streaming
may be necessary.