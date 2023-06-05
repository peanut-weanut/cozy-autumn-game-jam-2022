function GetBrowserVersion() {
    var userAgent = navigator.userAgent;
    var version = "Unknown";

    if (/rv:([\d.]+)/.test(userAgent)) {
        version = userAgent.match(/rv:([\d.]+)/)[1];
    } else if (/MSIE ([\d.]+)/.test(userAgent)) {
        version = userAgent.match(/MSIE ([\d.]+)/)[1];
    } else if (/Edge\/([\d.]+)/.test(userAgent)) {
        version = userAgent.match(/Edge\/([\d.]+)/)[1];
    } else if (/Chrome\/([\d.]+)/.test(userAgent)) {
        version = userAgent.match(/Chrome\/([\d.]+)/)[1];
    } else if (/Firefox\/([\d.]+)/.test(userAgent)) {
        version = userAgent.match(/Firefox\/([\d.]+)/)[1];
    } else if (/Safari\/([\d.]+)/.test(userAgent)) {
        version = userAgent.match(/Safari\/([\d.]+)/)[1];
    }

    return version;
}