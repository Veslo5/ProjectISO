	local scene = {}

	function scene.Initialize()
		print("Init from lua")
	end

	function scene.LoadContent()
		print("Load from lua")
	end

	function scene.Update()
		print("Update from lua")
	end

	function scene.Draw()
		print("Draw from lua")
	end

	return scene