Monospace.NoThreads.IteratorCoroutines
======================================

License
=======
Copyright (C) 2011 Arne F. Claassen

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

Contents
========
Program - contains the examples, can be run as a console app (running all examples) or individually via a NUnit test runner

IteratorExample - Example how IEnumerator<T> returning methods work

Coordinator - the coroutine coordinator, the class responsible for wiring up and coordinating coroutine calls

Producer - Coroutine reading form source array one row at a time and yielding to coordinator once a row is read

Consumer - Coroutine writing incoming sequence of ints into destination array

Exponentiator - Optional Coroutine, that hooks between producer and consumer to modify vales 

Coroutine "Shape"
=================
Each coroutine has to have a common "shape". It takes in a coordinator which it uses to yield control and return an enumerator so it can use yield return to suspend itself in favor of the coordinator. Each coroutine generally runs in an execution loop, occasionally yielding to the coordinator, usually when it has produced or consumed a certain amount of data.

  public IEnumerator SampleCoroutine(Coordinator<T> coordinator) {
    while(someCondition) {
      // do some work

      // yield execution to coordinator
      yield return null;
    }
  }

