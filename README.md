<script src="{{ site.baseurl }}/TemplateData/UnityProgress.js"></script>  
<script src="{{ site.baseurl }}/Build/UnityLoader.js"></script>
<script>
  var gameInstance = UnityLoader.instantiate("gameContainer", "{{ site.baseurl}}/Build/Build.json",{onProgress: UnityProgress});  
</script>
<div class="webgl-content">
  <div id="gameContainer" style="width: 860px; height: 600px"></div>
</div>

