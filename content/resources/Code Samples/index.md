---
layout: layout-container
showHeadings: false
order: 10
name: Code Samples
description: A listing of code samples for the UAT Automation Kit walkthroughs.
published: true
code:
  - title: Get Started with the UAT Automation Kit
    description: In this sample, you'll learn how to create a test project to get you started on the path to creating a ***Blackbaud CRM*** GUI test suite with the UAT Automation Kit.
    download: /assets/code-samples/201/ProjectBlue201.zip
    tutorial: <%= stache.config.blue_walkthroughs %>101/

  - title: UAT Automation Kit 201 Code Sample
    description: In the following walkthroughs, you'll learn how to use the UAT Automation Kit's underlying third-party tools to create custom logic and interactions with the browser.
    download: /assets/code-samples/201/ProjectBlue201.zip
    tutorial: <%= stache.config.blue_walkthroughs %>201/
---

{{ include 'includes/eapwarning/index.md' }}

# UAT Automation Kit Code Samples
<div class="code">

  <div class="clearfix"></div>

  {{# eachWithMod code mod=3 }}

    {{# if firstOrMod0 }}
      <div class="row">
    {{/ if }}
        <div class="col-sm-6 col-md-4">
          <div class="thumbnail">
             {{# if itemimage }}
               <img src="{{ itemimage }}" alt="" />
            {{/ if }}
            <div class="caption">
              <h3>{{ title }}</h3>
               {{# if description }}
                <p>{{ description }}</p>
              {{/ if }}
              <a href="{{download}}" class="btn btn-primary" role="button">Download Code</a>
              {{# if tutorial }}
                <a href="{{ tutorial }}" class="btn btn-white">
                  View Tutorial
                </a>
              {{/ if }}
            </div>
          </div>
        </div>
    {{# if lastOrMod1 }}
      </div>
    {{/ if }}

  {{/ eachWithMod }}

</div>


## test download

<a href="/assets/code-samples/201/ProjectBlue201.zip" download>Test Download</a>





