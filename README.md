# HashAlgorithmCrashReprod

Minimal reproducable example for [this issue](https://github.com/dotnet/runtime/issues/94569).

Although the example does not directly reproduce the issue, it possibly creates the catalyst. Deep cloning `TokenValidationParameters` -- and calling `ValidateToken` or `ValidateTokenAsync` with said parameters -- can potentially cause a `CryptographicException`.

Send multiple concurrent requests to the "run-test" endpoint via JMeter or a similar tool (the higher the number of requests the better). When the `deepCloneValidationParams` query parameter is false, no crashing occurs. When true, you will either get a `CryptographicException` or your IDE will crash.
