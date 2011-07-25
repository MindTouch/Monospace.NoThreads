Monospace.NoThreads.State
=========================

License
=======
Copyright (C) 2011 Arne F. Claassen

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

Contents
========
Program - contains the examples, can be run as a console app (running all examples) or individually via a NUnit test runner
AsyncMethodExample - Examples of async methods using either Task<T> or Result<T>
AsyncAdapter<T> - implementation of async access closure IAsyncAdapter
IAsyncAdapter<T> - Async access closure for any type T
SharingWithLocks - Example of traditional mechanism for guarantee serial access to a resource
SharingWithAsync - Example of using IAsyncAdapter to access a resource in a serial manner without locking the resource